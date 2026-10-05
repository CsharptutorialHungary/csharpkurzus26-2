namespace Calculator.Core;

public class NumberStack : INumberStack
{
    private readonly Stack<double> _stack = new();

    public int Count => _stack.Count;

    public double Pop() => _stack.Pop();

    public void Push(double number) => _stack.Push(number);
}