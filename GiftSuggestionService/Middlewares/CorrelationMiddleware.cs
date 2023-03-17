using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

public class CorrelationMiddleware
{
    private readonly RequestDelegate _next;

    public CorrelationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        context.Request.Headers.Add("correlation-id", Guid.NewGuid().ToString());
        await _next(context);
    }
}