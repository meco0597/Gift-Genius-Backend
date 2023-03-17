namespace GiftSuggestionService.Filters
{
    using System;
    using System.Net;
    using System.Net.Http.Headers;
    using System.Text;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Exception definition from the webservice
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Filters.IExceptionFilter" />
    public class ServiceExceptionFilter : IExceptionFilter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceExceptionFilter" /> class.
        /// </summary>
        /// <param name="logger">A logger instance.</param>
        public ServiceExceptionFilter()
        {
        }

        /// <summary>
        /// Convert the exception to response.
        /// </summary>
        /// <param name="context"></param>
        public void OnException(ExceptionContext context)
        {
            try
            {
                Exception exceptionToLog = context.Exception;
                HttpStatusCode status = HttpStatusCode.InternalServerError;
                context.ExceptionHandled = true;

                // Build HTTP resposne and return
                HttpResponse response = context.HttpContext.Response;
                response.StatusCode = (int)status;
                response.ContentType = "application/json";
                response.ContentType = new MediaTypeHeaderValue("application/json").ToString();
                response.WriteAsync(exceptionToLog.Message, Encoding.UTF8);

                if (status >= HttpStatusCode.InternalServerError)
                {
                    Console.Error.WriteLine("OnException: Unhandled Server Error", exceptionToLog);
                }
                else
                {
                    Console.Error.WriteLine("OnException: User error", exceptionToLog);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("OnException: Got an exception while trying to parse the context.", ex);
                Console.Error.WriteLine("OnException: Unhandled exception logged without parsing.", context?.Exception);
            }
        }
    }
}
