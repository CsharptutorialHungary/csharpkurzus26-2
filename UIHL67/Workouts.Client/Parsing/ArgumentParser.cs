using Workouts.Core.Exceptions;

namespace Workouts.Client.Parsing;

public static class ArgumentParser
{
    public static int ParseMinutes(string text) =>
        int.TryParse(text, out int minutes) && minutes > 0
            ? minutes
            : throw new InvalidCommandException($"A hossznak pozitív egész számnak kell lennie: '{text}'");

    public static int ParsePosition(string text) =>
        int.TryParse(text, out int position) && position > 0
            ? position
            : throw new InvalidCommandException($"A sorszámnak pozitív egész számnak kell lennie: '{text}'");
}
