using Workouts.Core;
using Workouts.Core.Exceptions;

namespace Workouts.Client.Parsing;

public static class CategoryParser
{
    private static readonly Dictionary<string, WorkoutCategory> _categories =
        Enum.GetValues<WorkoutCategory>()
            .ToDictionary(Name, StringComparer.OrdinalIgnoreCase);

    public static IEnumerable<string> All => _categories.Keys;

    public static string Name(WorkoutCategory category) => category.ToString().ToLowerInvariant();

    public static WorkoutCategory Parse(string text) =>
        _categories.TryGetValue(text, out var category)
            ? category
            : throw new InvalidCommandException($"Ismeretlen kategória: '{text}'.");

    public static WorkoutCategory? ParseOptional(string[] args, string usage)
    {
        if (args.Length > 1)
            throw new InvalidCommandException($"Használat: {usage}");

        return args.Length == 1 ? Parse(args[0]) : null;
    }
}
