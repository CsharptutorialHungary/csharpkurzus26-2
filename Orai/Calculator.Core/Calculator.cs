namespace Calculator.Core;

internal sealed class Calculator(ITokenizer tokenizer, INumberStack stack) : ICalculator
{
    public Either<double, Exception> Calculate(string input)
    {
        try
        {
            IToken[] tokens = tokenizer.Tokenize(input);
            foreach (IToken token in tokens)
            {
                token.Apply(stack);
            }
            return stack.Pop();
        }
        catch (Exception e)
        {
            return e;
        }
    }
}
