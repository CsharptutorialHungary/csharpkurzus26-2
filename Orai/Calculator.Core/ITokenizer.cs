using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator.Core;

internal interface ITokenizer
{
    IToken[] Tokenize(string input);
}

internal class RpnTokenizer : ITokenizer
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
            else if (double.TryParse(part, out double number))
            {
                //TODO: Add number token parsing here
            }
            //TODO: Throw exception or some error handling
        }
    }
}
