namespace Calculator.Core.Operations;

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