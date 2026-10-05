Calculator.Core.Calculator calculator = new();
while (true)
{
    Console.Write("input >");
    string? line = Console.ReadLine();
    if (!string.IsNullOrEmpty(line))
    {
        double d = calculator.Calculate(line);
        Console.WriteLine(d);
    }
}    