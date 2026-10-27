namespace Calculator.Core.Operations;

internal abstract class AggregateOperation : Operation
{
    public override void Apply(INumberStack stack)
    {
        if (stack.Count < 1)
        {
            throw new InvalidOperationException("Not enough values on stack");
        }

        IEnumerable<double> elements = stack.PopAll();

        double value = Apply(elements);
        stack.Push(value);

    }

    public abstract double Apply(IEnumerable<double> numbers);
}
