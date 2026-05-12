namespace MonitoriaBase.Models;

public class Student
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Email { get; init; }

    public Student(string name, string email)
    {
        Id = Guid.NewGuid();
        Name = name;
        Email = email;
    }
}
