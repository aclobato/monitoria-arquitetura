using MonitoriaBase.Services;
using Xunit;

namespace MonitoriaBase.Tests;

public class MonitoriaServiceTests
{
    private readonly MonitoriaService _service = new();

    [Fact]
    public void RegisterStudent_WithValidData_ReturnsStudentWithId()
    {
        var student = _service.RegisterStudent("Alice Silva", "alice@example.com");

        Assert.NotEqual(Guid.Empty, student.Id);
        Assert.Equal("Alice Silva", student.Name);
        Assert.Equal("alice@example.com", student.Email);
    }

    [Fact]
    public void GetStudents_AfterRegistering_ReturnsAllStudents()
    {
        _service.RegisterStudent("Alice", "alice@example.com");
        _service.RegisterStudent("Bob", "bob@example.com");

        var students = _service.GetStudents();

        Assert.Equal(2, students.Count);
    }

    [Fact]
    public void GetExercises_ReturnsSeededExercises()
    {
        var exercises = _service.GetExercises();

        Assert.NotEmpty(exercises);
    }

    [Fact]
    public void MarkExerciseAsCompleted_WithValidIds_ReturnsAttempt()
    {
        var student = _service.RegisterStudent("Alice", "alice@example.com");
        var exercise = _service.GetExercises()[0];

        var attempt = _service.MarkExerciseAsCompleted(student.Id, exercise.Id);

        Assert.Equal(student.Id, attempt.StudentId);
        Assert.Equal(exercise.Id, attempt.ExerciseId);
        Assert.Equal("Completed", attempt.Status);
    }

    [Fact]
    public void MarkExerciseAsCompleted_WithInvalidStudentId_ThrowsArgumentException()
    {
        var exercise = _service.GetExercises()[0];

        Assert.Throws<ArgumentException>(() =>
            _service.MarkExerciseAsCompleted(Guid.NewGuid(), exercise.Id));
    }

    [Fact]
    public void GetStudentProgress_ReturnsOnlyStudentAttempts()
    {
        var alice = _service.RegisterStudent("Alice", "alice@example.com");
        var bob = _service.RegisterStudent("Bob", "bob@example.com");
        var exercises = _service.GetExercises();

        _service.MarkExerciseAsCompleted(alice.Id, exercises[0].Id);
        _service.MarkExerciseAsCompleted(alice.Id, exercises[1].Id);
        _service.MarkExerciseAsCompleted(bob.Id, exercises[0].Id);

        var aliceProgress = _service.GetStudentProgress(alice.Id);

        Assert.Equal(2, aliceProgress.Count);
        Assert.All(aliceProgress, attempt => Assert.Equal(alice.Id, attempt.StudentId));
    }
}
