#region

using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Utils;
using Swashbuckle.AspNetCore.SwaggerGen;

#endregion

namespace PoliFemoBackend.Source.Configure;

public class ConfigureSwaggerOptions : IConfigureNamedOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
    {
        _provider = provider;
    }

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            var info = CreateVersionInfo(description);
            options.SwaggerDoc(description.GroupName, info);
            options.SupportNonNullableReferenceTypes();
            options.MapType<JToken>(() => new OpenApiSchema { Type = typeof(JToken).Name });
            options.OperationFilter<AuthOperationsFilter>();
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "Access token using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = Constants.Authorization,
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            options.TagActionsBy(api =>
            {
                if (api.GroupName != null) return new[] { api.GroupName };

                if (api.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
                    return new[] { controllerActionDescriptor.ControllerName };

                throw new InvalidOperationException("Unable to determine tag for endpoint.");
            });
            options.DocInclusionPredicate((name, api) => true);

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            ApiVersionsManager.AddVersion(info.Version);
        }
    }

    public void Configure(string name, SwaggerGenOptions options)
    {
        Configure(options);
    }

    private static OpenApiInfo CreateVersionInfo(ApiVersionDescription description)
    {
        var info = new OpenApiInfo
        {
            Title = "PoliFemoBackend API",
            Version = description.ApiVersion.ToString()
        };

        if (description.IsDeprecated) info.Description += " This API version has been deprecated.";

        return info;
    }
}