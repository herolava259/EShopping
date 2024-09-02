using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Basket.API.Swagger;

public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
    {
        this._provider = provider;
    }
    public void Configure(SwaggerGenOptions options)
    {
        foreach (var decription in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(decription.GroupName, ProvideApiInfo(decription));
        }
    }

    private OpenApiInfo ProvideApiInfo(ApiVersionDescription decription)
    {
        var info = new OpenApiInfo
        {
            Title = "Basket API Microservice",
            Version = decription.ApiVersion.ToString(),
            Description = "Fetch details about Basket",
            Contact = new OpenApiContact() { Name = "Farrer Le", Email = "elementalhero259@gmail.com" },
            License = new OpenApiLicense() { Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT")}
        };

        if(decription.IsDeprecated)
        {
            info.Description += " API version has been deprecated!!!";
        }

        return info;
    }
}
