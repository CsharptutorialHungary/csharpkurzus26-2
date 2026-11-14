namespace Workouts.Core;

public class WorkoutLog
{
    private readonly List<Workout> _workouts;

    public WorkoutLog()
    {
        _workouts = [];
    }

    public WorkoutLog(IEnumerable<Workout> workouts)
    {
        _workouts = [.. workouts];
    }

    public IReadOnlyList<Workout> Workouts => _workouts.AsReadOnly();

    public void Add(Workout workout) => _workouts.Add(workout);
    public IReadOnlyList<Workout> GetWorkouts(WorkoutCategory? filter)
    {
        return filter == null
        ? Workouts
        : [.. Workouts.Where(w => w.Category == filter)];
    }
    public IReadOnlyList<CategorySummary> GetSummary(WorkoutCategory? filter)
    {
        return [.. Enum.GetValues<WorkoutCategory>()
        .Where(c => filter == null || c == filter)
        .Select(c => new CategorySummary(c, Workouts.Where(w => w.Category == c).Sum(w => w.DurationMinutes)))];
    }
}

