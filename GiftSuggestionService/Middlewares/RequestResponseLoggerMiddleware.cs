using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

public class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestResponseLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        Console.WriteLine(FormatRequest(context.Request));

        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();

        await _next(context);

        stopWatch.Stop();
        var requestTimeInMilliseconds = stopWatch.ElapsedMilliseconds;

        Console.WriteLine(FormatResponse(context.Response, requestTimeInMilliseconds));
    }

    private string FormatRequest(HttpRequest request)
    {
        string correlationId = request.Headers["correlation-id"];
        return $"Incoming Request: CorrelationId={correlationId} Scheme={request.Scheme} RequestUrl={request.Host}{request.Path} Query={request.QueryString}";
    }

    private string FormatResponse(HttpResponse response, long elapsedMilliseconds)
    {
        string correlationId = response.Headers["correlation-id"];
        return $"Response: CorrelationId={correlationId} Latency={elapsedMilliseconds} Status={response.StatusCode}";
    }
}