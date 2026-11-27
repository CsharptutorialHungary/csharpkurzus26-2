using Workouts.Client.Commands;
using Workouts.Core;
using System.Text.Json;
using Workouts.Core.Exceptions;
using Workouts.Core.Persistence;

WorkoutRepository repository = new(Path.Combine(AppContext.BaseDirectory, "workouts.json"));

WorkoutLog log;
try
{
    log = new(repository.Load());
}
catch (Exception e) when (e is JsonException or IOException)
{
    Console.WriteLine($"Hiba a betöltéskor: {e.Message}.");
    log = new();
}

List<ICommand> commands = [new AddCommand(log, repository), new ListCommand(log), new SummaryCommand(log), new DeleteCommand(log, repository)];
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
        } catch (IOException e)
        {
            Console.WriteLine($"Hiba a mentéskor: {e.Message}.");
        }
    }
    if (line is null) break;
}    