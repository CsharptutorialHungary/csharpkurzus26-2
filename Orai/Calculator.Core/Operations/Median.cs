namespace Calculator.Core.Operations;

[TokenName("median")]
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
