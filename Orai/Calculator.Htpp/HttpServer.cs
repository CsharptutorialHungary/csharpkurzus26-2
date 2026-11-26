using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;

using Microsoft.Extensions.Logging;

namespace Calculator.Htpp;

public class HttpServer : IDisposable
{
    private readonly int _port;
    private readonly ILogger<HttpServer> _logger;
    private readonly TcpListener _server;
    private readonly SemaphoreSlim _semaphore;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly List<IRequestHandler> _handlers;
    private bool _disposed;

    public HttpServer(int port, ILoggerFactory loggerFactory)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(port, 1000);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(port, ushort.MaxValue);

        _port = port;
        _logger = loggerFactory.CreateLogger<HttpServer>();
        _server = new TcpListener(IPAddress.Any, _port);
        _semaphore = new SemaphoreSlim(10);
        _cancellationTokenSource = new CancellationTokenSource();
        _handlers = new List<IRequestHandler>();
    }

    public void RegisterHandler(IRequestHandler handler)
    {
        _handlers.Add(handler);
    }

    ~HttpServer()
    {
        Dispose(false);
    }

    protected virtual void Dispose(bool isDirectCall)
    {
        _server.Dispose();
        _semaphore.Dispose();
        _cancellationTokenSource.Dispose();
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public void Start()
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(HttpServer));

        _logger.LogInformation("Starting http server on port {port}", _port);
        _server.Start();
        Task.Run(ListenTask, _cancellationTokenSource.Token);
    }

    public void Stop()
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(HttpServer));

        _cancellationTokenSource.Cancel();
        _server.Stop();
    }

    private async Task ListenTask()
    {
        while (true)
        {
            _cancellationTokenSource.Token.ThrowIfCancellationRequested();
            TcpClient client = await _server.AcceptTcpClientAsync();
            await _semaphore.WaitAsync();
            //Munka
            try
            {
                _logger.LogInformation("Client connected: {ip}", client.Client.RemoteEndPoint);
                await HandleClient(client, _cancellationTokenSource.Token);
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }

    private async Task HandleClient(TcpClient client, CancellationToken token)
    {
        using var stream = client.GetStream();
        try
        {
            HttpRequest request = await HttpRequestParser.ParseAsync(stream, _port);
            foreach (var handler in _handlers)
            {
                if (await handler.Handle(request, stream, token))
                {
                    _logger.LogInformation("Running handler: {handler}", handler.GetType());
                    return;
                }
            }
            _logger.LogWarning("Not found: {url}", request.Path);
            await SpecialHandlers.HandleNotFound(stream);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error serving request");
            await SpecialHandlers.HandleServerError(stream, ex);
        }
    }
}

public static class HttpRequestParser
{
    private enum ParserState
    {
        FirstLine,
        Headers,
        Body,
    }

    public static async ValueTask<HttpRequest> ParseAsync(Stream stream, int port)
    {
        using var reader = new StreamReader(stream, leaveOpen: true);
        StringBuilder body = new();
        string? line;
        ParserState state = ParserState.FirstLine;

        var resultRequest = new HttpRequest();

        while ((line = await reader.ReadLineAsync()) != null)
        {
            if (state == ParserState.FirstLine)
            {
                var parts = line.Split(' ', 3, StringSplitOptions.TrimEntries);
                if (parts.Length < 3)
                    throw new InvalidOperationException("Invalid request line format.");

                resultRequest.Method = Enum.Parse<Method>(parts[0], ignoreCase: true);
                resultRequest.Path = new Uri($"http://localhost:{port}{parts[1]}", UriKind.Absolute);
                resultRequest.Version = parts[2];
                state = ParserState.Headers;
            }
            else if (state == ParserState.Headers)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    if (resultRequest.Method == Method.Get)
                    {
                        break; // No body for GET requests
                    }
                    // Empty line indicates the end of headers
                    state = ParserState.Body;
                    continue;
                }

                var headerParts = line.Split(':', 2, StringSplitOptions.TrimEntries);

                if (headerParts.Length != 2)
                    throw new InvalidOperationException("Invalid header format.");

                resultRequest.Headers[headerParts[0]] = headerParts[1];
            }
            else if (state == ParserState.Body)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    break; // End of body
                }
                body.AppendLine(line);
            }
        }

        if (body.Length > 0)
        {
            resultRequest.Body = body.ToString().TrimEnd();
        }


        return resultRequest;
    }
}

public enum Method
{
    Get,
    Post,
    Put,
    Delete,
    Patch,
    Head,
    Options
}

public sealed class HttpRequest
{
    public Method Method { get; set; }
    public Uri Path { get; set; }
    public string Version { get; set; }
    public OrderedDictionary<string, string> Headers { get; set; }
    public string? Body { get; set; }

    public HttpRequest()
    {
        Headers = new OrderedDictionary<string, string>();
        Version = "HTTP/1.1";
        Path = new Uri("/", UriKind.Relative);
    }

    public long? Length
    {
        get
        {
            return Headers.TryGetValue("Content-Length", out string? value)
                ? long.Parse(value)
                : null;
        }
    }

    public override string ToString()
        => $"{Method} {Path} {Version}";
}

public interface IRequestHandler
{
    Task<bool> Handle(HttpRequest request, NetworkStream responseStream, CancellationToken cancellationToken);
}

public abstract class JsonRequestHandler<TResponse> : IRequestHandler
{
    private readonly string _requestPath;

    public JsonRequestHandler(string reqestPath)
    {
        _requestPath = reqestPath;
    }

    protected abstract Task<(TResponse response, HttpStatusCode responseCode)> Handle(HttpRequest request);

    public async Task<bool> Handle(HttpRequest request, NetworkStream responseStream, CancellationToken cancellationToken)
    {
        if (request.Path.LocalPath != _requestPath)
            return false;

        (TResponse response, HttpStatusCode responseCode) response = await Handle(request);

        string json = JsonSerializer.Serialize(response.response, JsonSerializerOptions.Web);

        string httpResponse = $"""
            HTTP/1.1 {(int)response.responseCode} {response.responseCode}
            Date: {DateTime.UtcNow:R}
            Content-Type: application/json; charset=utf-8
            Content-Length: {Encoding.UTF8.GetByteCount(json)}

            {json}
            """;

        var encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);

        using StreamWriter writer = new StreamWriter(responseStream, encoding, leaveOpen: true);

        await writer.WriteAsync(httpResponse);

        return true;
    }
}