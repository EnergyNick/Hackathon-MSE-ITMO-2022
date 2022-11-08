namespace StudentManager.Service.Extensions;

public static class EnvironmentExtensions
{
    private static readonly string TestingEnvName = "Testing";

    public static bool IsTesting(this IHostEnvironment hostEnvironment)
    {
        if (hostEnvironment == null)
        {
            throw new ArgumentNullException(nameof(hostEnvironment));
        }

        return hostEnvironment.IsEnvironment(TestingEnvName);
    }
}