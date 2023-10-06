#region

using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

#endregion

namespace PoliFemoBackend.Source.Utils.Auth;

// ReSharper disable once ClassNeverInstantiated.Global
public class AuthOperationsFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var noAuthRequired = context.ApiDescription
            .CustomAttributes()
            .All(attr => attr.GetType() != typeof(AuthorizeAttribute));

        if (noAuthRequired)
            return;

        operation.Security = new List<OpenApiSecurityRequirement>
        {
            new()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            }
        };
    }
}
