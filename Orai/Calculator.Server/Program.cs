using Calculator.Htpp;
using Calculator.Core;

using Microsoft.Extensions.Logging;

using System.Net;
using System.Web;
using System.Globalization;
using System.Diagnostics;

using ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

using var server = new HttpServer(8080, loggerFactory);
server.RegisterHandler(new CalculateHandler());

server.Start();

Console.ReadKey();

server.Stop();

internal class CalculateHandler : JsonRequestHandler<string>
{
    private readonly ICalculator calculator;

    public CalculateHandler() : base("/calculate")
    {
        calculator = CalculatorFactory.Create();
    }

    protected override async Task<(string response, HttpStatusCode responseCode)> Handle(HttpRequest request)
    {
        var parameters = HttpUtility.ParseQueryString(request.Path.Query);

        var queryString = parameters.Get("q");
        if (string.IsNullOrWhiteSpace(queryString))
        {
            return ("bad request", HttpStatusCode.BadRequest);
        }

        Either<double, Exception> result = calculator.Calculate(queryString);

        if (result.TryGetSuccess(out double number))
        {
            return (number.ToString(CultureInfo.InvariantCulture), HttpStatusCode.OK);
        }

        if (result.TryGetError(out Exception? exception))
        {
            return (exception.Message, HttpStatusCode.InternalServerError);
        }

        throw new UnreachableException();
    }
}