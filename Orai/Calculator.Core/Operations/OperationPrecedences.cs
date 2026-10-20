namespace Calculator.Core.Operations;

internal static class OperationPrecedences
{
    public const int AdditionSubtractionPrecedence = 1;
    public const int MultiplyDivisionPrecedence = 2;

    public const int Function = int.MaxValue;
}
