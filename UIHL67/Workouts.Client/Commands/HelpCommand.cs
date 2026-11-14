using Workouts.Client.Parsing;
using Workouts.Core.Exceptions;

namespace Workouts.Client.Commands;

public class HelpCommand(IEnumerable<ICommand> commands) : ICommand
{
    public string Name => "help";
    public string Usage => "help";
    public string Description => "ez a súgó";

    public string Execute(string[] args)
    {
        if (args.Length != 0)
            throw new InvalidCommandException($"Használat: {Usage}");

        return "Parancsok:" + Environment.NewLine +
            string.Join(Environment.NewLine, commands.Select(c => $"  {c.Usage,-25}{c.Description}")) +
            Environment.NewLine + $"Kategóriák: {string.Join(", ", CategoryParser.All)}";
    }
}
