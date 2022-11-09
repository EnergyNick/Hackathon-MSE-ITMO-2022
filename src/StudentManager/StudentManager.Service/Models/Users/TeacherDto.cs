namespace StudentManager.Service.Models.Users;

public record TeacherDto
{
    public string Id { get; init; }
    public string TelegramId { get; init; }
    public string? Email { get; init; }

    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string? Patronymic { get; init; }
}