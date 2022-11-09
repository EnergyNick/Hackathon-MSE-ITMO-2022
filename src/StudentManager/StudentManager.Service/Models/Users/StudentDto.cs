namespace StudentManager.Service.Models.Users;

public record StudentDto
{
    public string Id { get; init; }
    public string TelegramId { get; init; }

    public string IsuId { get; init; }
    public string GroupId { get; init; }

    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string? Patronymic { get; init; }
}