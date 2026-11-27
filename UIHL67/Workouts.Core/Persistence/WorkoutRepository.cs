using System.Text.Json;
using System.Text.Json.Serialization;

namespace Workouts.Core.Persistence;

public class WorkoutRepository(string filePath)
{
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() },
    };

    public List<Workout> Load()
    {
        if (!File.Exists(filePath)) return [];

        string json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<List<Workout>>(json, Options) ?? [];
    }

    public void Save(IEnumerable<Workout> workouts)
    {
        string json = JsonSerializer.Serialize(workouts, Options);
        File.WriteAllText(filePath, json);
    }
}
