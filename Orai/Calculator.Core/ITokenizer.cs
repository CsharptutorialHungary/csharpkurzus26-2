namespace Calculator.Core;

internal interface ITokenizer
{
    IToken[] Tokenize(string input);
}
