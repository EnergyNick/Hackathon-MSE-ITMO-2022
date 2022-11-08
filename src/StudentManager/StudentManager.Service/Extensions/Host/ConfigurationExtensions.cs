using StudentManager.Core.Exceptions;

namespace StudentManager.Service.Extensions
{
    public static class ConfigurationExtensions
    {
        public static T GetNotNull<T>(this IConfiguration configuration, string? key = null)
        {
            key ??= typeof(T).Name;

            var section = configuration.GetSection(key);
            if (section.Value == null && !section.GetChildren().Any())
                throw new ConfigurationException($"Configuration file doesn't contain '{key}' section");

            return section.Get<T>();
        }
    }
}