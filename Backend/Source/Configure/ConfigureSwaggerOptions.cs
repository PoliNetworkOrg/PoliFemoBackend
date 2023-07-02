#region

using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Linq;
using PoliFemoBackend.Source.Data;
using PoliFemoBackend.Source.Utils.Auth;
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
        options.SwaggerDoc("definitions", new OpenApiInfo
        {
            Title = "PoliFemo API",
            Version = "v1",
            Description = "PoliFemo API",
            Contact = new OpenApiContact
            {
                Name = "PoliFemo",
                Email = "dsd"
            }
        });

        options.SupportNonNullableReferenceTypes();
        options.MapType<JToken>(() => new OpenApiSchema { Type = nameof(JToken) });
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
    }

    public void Configure(string name, SwaggerGenOptions options)
    {
        Configure(options);
    }
}