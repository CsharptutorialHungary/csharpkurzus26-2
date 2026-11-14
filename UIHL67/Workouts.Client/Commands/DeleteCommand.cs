using Workouts.Client.Parsing;
using Workouts.Core;
using Workouts.Core.Exceptions;
using Workouts.Core.Persistence;

namespace Workouts.Client.Commands;

public class DeleteCommand(WorkoutLog log, WorkoutRepository repository) : ICommand
{
    public string Name => "delete";
    public string Usage => "delete [kategória] <sorszám>";
    public string Description => "edzés törlése a list szerinti sorszámmal";

    public string Execute(string[] args)
    {
        if (args.Length < 1 || args.Length > 2)
            throw new InvalidCommandException($"Használat: {Usage}");

        WorkoutCategory? category = args.Length == 2 ? CategoryParser.Parse(args[0]) : null;
        int position = ArgumentParser.ParsePosition(args[^1]);

        int count = log.GetWorkouts(category).Count;
        if (position > count)
        {
            string scope = category is null ? "összesen" : CategoryParser.Name(category.Value) + ":";
            throw new InvalidCommandException($"Nincs {position}. edzés ({scope} {count} db)");
        }

        Workout removed = log.Remove(category, position);
        repository.Save(log.Workouts);
        return $"Edzés törölve: {CategoryParser.Name(removed.Category)}, {removed.DurationMinutes} perc";
    }
}
