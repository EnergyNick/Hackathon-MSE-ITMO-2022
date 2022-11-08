using Serilog;
using ILogger = Serilog.ILogger;

namespace StudentManager.Service.Logger
{
    public static class LoggerBuilder
    {
        public static ILogger CreateLogger()
        {
            return new LoggerConfiguration()
                .WriteTo.Console(outputTemplate:BuildLogTemplate())
                .Enrich.WithThreadId()
                .Enrich.FromLogContext()
                .CreateLogger();
        }

        private static string BuildLogTemplate()
        {
            return "{Timestamp:yyyy-MM-dd hh:mm:ss.fff}" +
                   " {Level:u3}" +
                   " {ThreadId}" +
                   " [{SourceContext}]" +
                   " {Message}{NewLine}{Exception}";
        }
    }
}