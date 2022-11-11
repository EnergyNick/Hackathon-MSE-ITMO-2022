namespace StudentManager.Core.Hosting
{
    public interface IPreInitializationService
    {
        Task InitializeAsync();
    }
}