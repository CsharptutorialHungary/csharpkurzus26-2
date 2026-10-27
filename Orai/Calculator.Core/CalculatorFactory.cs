namespace Calculator.Core;

public static class CalculatorFactory
{
    public static ICalculator Create()
    {
        ITokenizer tokenizer = new RpnTokenizer(new TokenProvider());
        INumberStack stack = new NumberStack();

        return new Calculator(tokenizer, stack);
    }
}
