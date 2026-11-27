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

    public Workout Remove(WorkoutCategory? filter, int position)
    {
        List<int> matchingIndexes = [.. _workouts
            .Select((workout, index) => (workout, index))
            .Where(x => filter == null || x.workout.Category == filter)
            .Select(x => x.index)];

        if (position < 1 || position > matchingIndexes.Count)
            throw new ArgumentOutOfRangeException(nameof(position));

        int index = matchingIndexes[position - 1];
        Workout removed = _workouts[index];
        _workouts.RemoveAt(index);
        return removed;
    }

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

