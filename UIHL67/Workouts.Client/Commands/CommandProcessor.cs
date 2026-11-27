using Workouts.Core.Exceptions;

namespace Workouts.Client.Commands;

public class CommandProcessor(IEnumerable<ICommand> commands)
{
    private readonly Dictionary<string, ICommand> _commands =
        commands.ToDictionary(c => c.Name, StringComparer.OrdinalIgnoreCase);

    public string Process(string input)
    {
        string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 0)
            throw new InvalidCommandException("Üres parancs");

        return _commands.TryGetValue(parts[0], out var command)
            ? command.Execute(parts[1..])
            : throw new InvalidCommandException($"Ismeretlen parancs: '{parts[0]}'. Írd be: help");
    }
}
