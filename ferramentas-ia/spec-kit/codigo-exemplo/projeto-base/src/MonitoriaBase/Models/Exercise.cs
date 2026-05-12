namespace MonitoriaBase.Models;

public class Exercise
{
    public Guid Id { get; init; }
    public string Title { get; init; }
    public string Topic { get; init; }
    public string DifficultyLevel { get; init; }

    public Exercise(string title, string topic, string difficultyLevel)
    {
        Id = Guid.NewGuid();
        Title = title;
        Topic = topic;
        DifficultyLevel = difficultyLevel;
    }
}
