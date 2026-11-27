using Workouts.Client.Parsing;
using Workouts.Core;
using Workouts.Core.Exceptions;
using Workouts.Core.Persistence;

namespace Workouts.Client.Commands;

public class AddCommand(WorkoutLog log, WorkoutRepository repository) : ICommand
{
    public string Name => "add";
    public string Usage => "add <kategória> <perc>";
    public string Description => "új edzés rögzítése";

    public string Execute(string[] args)
    {
        if (args.Length != 2)
            throw new InvalidCommandException($"Használat: {Usage}");

        var category = CategoryParser.Parse(args[0]);
        var minutes = ArgumentParser.ParseMinutes(args[1]);
        log.Add(new Workout(category, minutes));
        repository.Save(log.Workouts);
        return $"Edzés rögzítve: {CategoryParser.Name(category)}, {minutes} perc";
    }
}
