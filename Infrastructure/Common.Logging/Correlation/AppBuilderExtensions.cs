using Microsoft.AspNetCore.Builder;

namespace Common.Logging.Correlation;

public static class AppBuilderExtensions
{
    public static IApplicationBuilder AddCorrelationIdMiddleWare(this IApplicationBuilder appBuilder)
        => appBuilder.UseMiddleware<CorrelationIdMiddleware>();
}
