namespace StudentManager.Tables;

public static class ParserUtilities
{
    public static string GetFCs(this IPerson person) =>
        $"{person.Surname} {person.Name}" +
        (person.Patronymic != "" ? $" {person.Patronymic}" : "");
}