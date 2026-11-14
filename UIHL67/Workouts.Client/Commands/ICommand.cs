namespace Workouts.Client.Commands;

public interface ICommand
{
    string Name { get; }
    string Usage { get; }
    string Description { get; }
    string Execute(string[] args);
}
