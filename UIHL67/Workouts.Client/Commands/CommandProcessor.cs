using Workouts.Core;
using Workouts.Core.Exceptions;

namespace Workouts.Client.Commands;

public class CommandProcessor
{
    public string Process(string input)
    {
        string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        return parts switch
        {
            ["add", var category, var minutes] => Add(ParseCategory(category), ParseMinutes(minutes)),
            ["list"] => List(null),
            ["list", var category] => List(ParseCategory(category)),
            ["summary"] => Summary(null),
            ["summary", var category] => Summary(ParseCategory(category)),
            ["help"] => Help(),
            [var command, ..] => throw new InvalidCommandException($"Ismeretlen parancs: '{command}'. Írd be: help"),
            [] => throw new InvalidCommandException("Üres parancs"),
        };
    }

    private string Add(WorkoutCategory category, int minutes) => throw new NotImplementedException();

    private string List(WorkoutCategory? category) => throw new NotImplementedException();

    private string Summary(WorkoutCategory? category) => throw new NotImplementedException();

    private string Help() => throw new NotImplementedException();

    private WorkoutCategory ParseCategory(string text) => throw new NotImplementedException();

    private int ParseMinutes(string text) => throw new NotImplementedException();
}
