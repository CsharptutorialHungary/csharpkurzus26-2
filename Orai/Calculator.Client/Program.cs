using Calculator.Core;

ICalculator calculator = CalculatorFactory.Create();

while (true)
{
    Console.Write("input >");
    string? line = Console.ReadLine();
    if (!string.IsNullOrEmpty(line))
    {
        Either<double, Exception> result = calculator.Calculate(line);

        if (result.TryGetSuccess(out double validResult))
        {
            Console.WriteLine(validResult);
        }
        else if (result.TryGetError(out Exception? ex))
        {
            Console.WriteLine(ex.Message);
        }
    }
}    