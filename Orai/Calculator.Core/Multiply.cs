namespace Calculator.Core;

public sealed class Multiply : BinaryOperation
{
    public override int Precedence { get; }
        = OperationPrecedences.AdditionSubtractionPrecedence;

    protected override double Apply(double left, double right)
    {
        return left * right;
    }
}
