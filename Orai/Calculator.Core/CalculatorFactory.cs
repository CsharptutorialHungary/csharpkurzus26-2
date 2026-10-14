namespace Calculator.Core;

public class CalculatorFactory
{
    public static ICalculator Create()
    {
        ITokenizer tokenizer = new RpnTokenizer();
        INumberStack stack = new NumberStack();

        return new Calculator(tokenizer, stack);
    }
}
