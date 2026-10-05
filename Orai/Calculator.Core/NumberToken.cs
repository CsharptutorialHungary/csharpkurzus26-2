namespace Calculator.Core;

internal class NumberToken(double number) : IToken
{
    public void Apply(INumberStack stack)
        => stack.Push(number);
}