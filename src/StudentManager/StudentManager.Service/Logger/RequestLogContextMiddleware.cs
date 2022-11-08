using System.Diagnostics;
using Microsoft.Net.Http.Headers;
using Serilog;
using ILogger = Serilog.ILogger;

namespace StudentManager.Service.Logger;

public class LoggingMiddleware : IMiddleware
{
    private readonly ILogger _log = Log.ForContext<LoggingMiddleware>();

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var request = context.Request;

        var requestInfo = $"[{request.Method} {request.Path.Value}{request.QueryString.Value}]";
        _log.Information("Request start: {RequestInfo} {ContentType} {ContentLength}",
            requestInfo,
            request.ContentType,
            request.ContentLength);

        var stopwatch = Stopwatch.StartNew();
        await next(context);
        stopwatch.Stop();

        var response = context.Response;
        if (response.StatusCode is >= 300 and < 400)
            _log.Information("Redirected to: {Location}", response.Headers[HeaderNames.Location]);
        _log.Information("Request end: Elapsed {ElapsedMs}ms {StatusCode} {RequestInfo} {ContentType} {ContentLength}",
            stopwatch.ElapsedMilliseconds,
            response.StatusCode,
            requestInfo,
            response.ContentType,
            response.ContentLength);
    }
}