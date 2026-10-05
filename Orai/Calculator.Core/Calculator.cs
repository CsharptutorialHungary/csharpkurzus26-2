using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator.Core;

public class Calculator
{
    private readonly ITokenizer _tokenizer;
    private readonly INumberStack _stack;

    public Calculator()
    {
        _tokenizer = new RpnTokenizer();
        _stack = new NumberStack();
    }

    public double Calculate(string input)
    {
        IToken[] tokens = _tokenizer.Tokenize(input);
        foreach (IToken token in tokens)
        {
            token.Apply(_stack);
        }
        return _stack.Pop();
    }
}
