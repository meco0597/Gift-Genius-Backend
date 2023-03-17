using Microsoft.AspNetCore.Http;

namespace GiftSuggestionService.Services
{
    public class RequestAccessorService
    {
        private readonly IHttpContextAccessor httpContext;

        public RequestAccessorService(IHttpContextAccessor httpContext)
        {
            this.httpContext = httpContext;
        }

        public string GetCorrelationId()
        {
            return this.httpContext.HttpContext.Request.Headers["correlation-id"];
        }
    }
}