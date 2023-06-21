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
        string correlationId = Guid.NewGuid().ToString();
        context.Request.Headers.Add("correlation-id", correlationId);
        Console.WriteLine(FormatRequest(context.Request, correlationId));

        Stopwatch stopWatch = new Stopwatch();
        stopWatch.Start();

        await _next(context);

        stopWatch.Stop();
        var requestTimeInMilliseconds = stopWatch.ElapsedMilliseconds;

        Console.WriteLine(FormatResponse(context.Response, requestTimeInMilliseconds, correlationId));
    }

    private string FormatRequest(HttpRequest request, string correlationId)
    {
        return $"Incoming Request: Scheme={request.Scheme} Method={request.Method} RequestUrl={request.Host}{request.Path} Query={request.QueryString} CorrelationId={correlationId}";
    }

    private string FormatResponse(HttpResponse response, long elapsedMilliseconds, string correlationId)
    {
        return $"Response: Latency={elapsedMilliseconds} Status={response.StatusCode} CorrelationId={correlationId}";
    }
}