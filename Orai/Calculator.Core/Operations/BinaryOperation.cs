namespace Calculator.Core.Operations;

internal abstract class BinaryOperation : Operation
{
    public override void Apply(INumberStack stack)
    {
        if (stack.Count < 2)
        {
            throw new InvalidOperationException("Not enough values on the stack.");
        }

        double right = stack.Pop();
        double left = stack.Pop();

        double result = Apply(left, right);

        stack.Push(result);
    }

    protected abstract double Apply(double left, double right);
}

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

internal class Median : AggregateOperation
{
    public override int Precedence => OperationPrecedences.Function;

    public override double Apply(IEnumerable<double> numbers)
    {
        var query = from number in numbers
                    orderby number ascending
                    select number;

        double[] array = query.ToArray();

        int midleIndex = array.Length / 2;

        return array.Length % 2 == 0
            ? array.Skip(midleIndex - 1).Take(2).Average()
            : array[midleIndex];
    }
}

internal static class Extensions
{
    public static IEnumerable<double> PopAll(this INumberStack numberStack)
    {
        while (numberStack.Count > 0)
        {
            yield return numberStack.Pop();
        }
    }
}