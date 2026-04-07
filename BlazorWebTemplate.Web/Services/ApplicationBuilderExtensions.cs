using BlazorWebTemplate.Client.Services.BackEnd.Infrastructure.Http;

namespace BlazorWebTemplate.Web.Services;

public static class ApplicationBuilderExtensions
{
    public static WebApplication UseBlazorWebTemplateWeb(this WebApplication app)
    {
        app.Use(async (context, next) =>
        {
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
