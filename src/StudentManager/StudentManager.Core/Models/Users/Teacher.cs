namespace StudentManager.Core.Models.Users;

public record Teacher
{
    public string Id { get; init; }
    public string TelegramId { get; init; }
    public string? Email { get; init; }

    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string? Patronymic { get; init; }
}