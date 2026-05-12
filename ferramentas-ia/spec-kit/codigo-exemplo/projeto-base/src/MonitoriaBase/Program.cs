using MonitoriaBase.Services;

var service = new MonitoriaService();

// Registrar alunos
var alice = service.RegisterStudent("Alice Silva", "alice@example.com");
var bob = service.RegisterStudent("Bob Santos", "bob@example.com");

// Listar exercícios disponíveis
var exercises = service.GetExercises();
Console.WriteLine($"Exercícios disponíveis: {exercises.Count}");

foreach (var exercise in exercises)
    Console.WriteLine($"  [{exercise.DifficultyLevel,14}] {exercise.Title} — {exercise.Topic}");

// Registrar conclusão de exercícios
service.MarkExerciseAsCompleted(alice.Id, exercises[0].Id);
service.MarkExerciseAsCompleted(alice.Id, exercises[1].Id);
service.MarkExerciseAsCompleted(bob.Id, exercises[0].Id);

// Ver progresso dos alunos
foreach (var student in service.GetStudents())
{
    var progress = service.GetStudentProgress(student.Id);
    Console.WriteLine($"\n{student.Name}: {progress.Count} exercício(s) concluído(s)");
}
