using System.Globalization;

namespace Calculator.Core;

internal sealed class RpnTokenizer : ITokenizer
{
    private readonly ITokenProvider _tokenProvider;

    public RpnTokenizer(ITokenProvider tokenProvider)
    {
        _tokenProvider = tokenProvider;
    }

    public IToken[] Tokenize(string input)
    {
        string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        List<IToken> tokens = new();
        foreach (string part in parts)
        {
            if (_tokenProvider.Tokens.TryGetValue(part, out IToken? token))
            {
                tokens.Add(token);
            }
            else if (double.TryParse(part, CultureInfo.InvariantCulture, out double number))
            {
                tokens.Add(new NumberToken(number));
            }
            else
            {
                throw new InvalidOperationException($"Invalid token: {part}");
            }
        }
        return tokens.ToArray();
    }
}
