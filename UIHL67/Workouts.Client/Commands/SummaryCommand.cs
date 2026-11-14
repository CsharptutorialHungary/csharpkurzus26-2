using Workouts.Client.Parsing;
using Workouts.Core;

namespace Workouts.Client.Commands;

public class SummaryCommand(WorkoutLog log) : ICommand
{
    public string Name => "summary";
    public string Usage => "summary [kategória]";
    public string Description => "össz. hossz kategóriánként (vagy egy kategóriára)";

    public string Execute(string[] args)
    {
        var category = CategoryParser.ParseOptional(args, Usage);

        return string.Join(Environment.NewLine,
            log.GetSummary(category).Select(s => $"{CategoryParser.Name(s.Category)}: {s.TotalMinutes} perc"));
    }
}
