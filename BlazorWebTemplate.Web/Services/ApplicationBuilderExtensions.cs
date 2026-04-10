using BlazorWebTemplate.Client.Services.BackEnd.Infrastructure.Http;

namespace BlazorWebTemplate.Web.Services;

public static class ApplicationBuilderExtensions
{
    public static WebApplication UseBlazorWebTemplateWeb(this WebApplication app)
    {
        app.Use(async (context, next) =>
        {
            // Security headers
            context.Response.Headers["X-Content-Type-Options"] = "nosniff";
            context.Response.Headers["X-Frame-Options"] = "DENY";
            context.Response.Headers["Referrer-Policy"] = "strict-origin-when-cross-origin";
            context.Response.Headers["Permissions-Policy"] = "camera=(), microphone=(), geolocation=()";

            // Correlation ID propagation
            var correlationId = context.Request.Headers[CorrelationIdHandler.HeaderName].FirstOrDefault();
            if (string.IsNullOrWhiteSpace(correlationId))
            {
                correlationId = context.TraceIdentifier;
                context.Request.Headers[CorrelationIdHandler.HeaderName] = correlationId;
            }

            context.Response.Headers[CorrelationIdHandler.HeaderName] = correlationId;

            using var scope = app.Logger.BeginScope(new Dictionary<string, object?>
            {
                ["CorrelationId"] = correlationId,
                ["Path"] = context.Request.Path.Value
            });

            await next();
        });

        return app;
    }
}
