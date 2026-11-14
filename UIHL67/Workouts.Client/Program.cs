using Workouts.Client.Commands;
using Workouts.Core;
using Workouts.Core.Exceptions;

WorkoutLog log = new();
List<ICommand> commands = [new AddCommand(log), new ListCommand(log), new SummaryCommand(log)];
commands.Add(new HelpCommand(commands));
CommandProcessor cp = new(commands);

while (true)
{
    Console.Write("input >");
    string? line = Console.ReadLine();
    if (!string.IsNullOrEmpty(line))
    {
        try
        {
            Console.WriteLine(cp.Process(line));
        } catch (InvalidCommandException e)
        {
            Console.WriteLine(e.Message);
        }
    }
    if (line is null) break;
}    