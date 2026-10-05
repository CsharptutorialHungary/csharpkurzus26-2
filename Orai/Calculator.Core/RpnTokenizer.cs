using System.Globalization;

using Calculator.Core.Operations;

namespace Calculator.Core;

internal sealed class RpnTokenizer : ITokenizer
{
    private readonly Dictionary<string, IToken> _operators;

    public RpnTokenizer()
    {
        _operators = new Dictionary<string, IToken>
        {
            { "+", new Addition() },
            { "-", new Subtraction() },
            { "*", new Multiply() },
            { "/", new Division() }
        };
    }

    public IToken[] Tokenize(string input)
    {
        string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        List<IToken> tokens = new();
        foreach (string part in parts)
        {
            if (_operators.TryGetValue(part, out IToken? token))
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
