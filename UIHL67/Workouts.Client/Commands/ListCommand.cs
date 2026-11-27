using Workouts.Client.Parsing;
using Workouts.Core;

namespace Workouts.Client.Commands;

public class ListCommand(WorkoutLog log) : ICommand
{
    public string Name => "list";
    public string Usage => "list [kategória]";
    public string Description => "edzések felvételi sorrendben (vagy egy kategóriáé)";

    public string Execute(string[] args)
    {
        var category = CategoryParser.ParseOptional(args, Usage);

        var workouts = log.GetWorkouts(category);
        if (workouts.Count == 0)
            return $"Nincs rögzített edzés{(category is null ? "" : $": {CategoryParser.Name(category.Value)}")}";

        return string.Join(Environment.NewLine,
            workouts.Select((w, i) => $"{i + 1}. {CategoryParser.Name(w.Category)} - {w.DurationMinutes} perc"));
    }
}
