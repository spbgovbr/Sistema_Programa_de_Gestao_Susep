using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Susep.SISRH.ApiGateway.Middlewares
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;
        private readonly RequestDelegate _next;
        public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            context.Request.EnableBuffering();
            var builder = new StringBuilder();
            var request = await FormatRequest(context.Request);
            builder.Append("Request: ").AppendLine(request);
            builder.AppendLine("Request headers:");
            foreach (var header in context.Request.Headers)
            {
                builder.Append(header.Key).Append(':').AppendLine(header.Value);
            }
            //Copy a pointer to the original response body stream
            var originalBodyStream = context.Response.Body;
            //Create a new memory stream...
            using (var responseBody = new MemoryStream())
            {
                //...and use that for the temporary response body
                context.Response.Body = responseBody;
                //Continue down the Middleware pipeline, eventually returning to this class
                await _next(context);
                //Format the response from the server
                var response = await FormatResponse(context.Response);
                builder.Append("Response: ").AppendLine(response);
                builder.AppendLine("Response headers: ");
                foreach (var header in context.Response.Headers)
                {
                    builder.Append(header.Key).Append(':').AppendLine(header.Value);
                }
                //Save log to chosen datastore
                _logger.LogInformation(builder.ToString());
                //Copy the contents of the new memory stream (which contains the response) to the original stream, which is then returned to the client.
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }
        private async Task<string> FormatRequest(HttpRequest request)
        {
            // Leave the body open so the next middleware can read it.
            using (var reader = new StreamReader(
                 request.Body,
                 encoding: Encoding.UTF8,
                 detectEncodingFromByteOrderMarks: false,
                 1024,
                 leaveOpen: true))
            {
                var body = await reader.ReadToEndAsync();
                // Do some processing with body…
                var formattedRequest = $"{request.Scheme} {request.Host}{request.Path} {request.QueryString} {body}";
                // Reset the request body stream position so the next middleware can read it
                request.Body.Position = 0;

                return formattedRequest;
            }
        }
        
        private async Task<string> FormatResponse(HttpResponse response)
        {
            //We need to read the response stream from the beginning...
            response.Body.Seek(0, SeekOrigin.Begin);
            //...and copy it into a string
            string text = await new StreamReader(response.Body).ReadToEndAsync();
            //We need to reset the reader for the response so that the client can read it.
            response.Body.Seek(0, SeekOrigin.Begin);
            //Return the string for the response, including the status code (e.g. 200, 404, 401, etc.)
            return $"{response.StatusCode}: {text}";
        }
    }
}
