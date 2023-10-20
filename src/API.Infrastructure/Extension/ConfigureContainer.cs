using API.Service.Middleware;
using Microsoft.AspNetCore.Builder;

namespace API.Infrastructure.Extension
{
    public static class ConfigureContainer
    {
        public static void ConfigureMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<CustomExceptionMiddleware>();
        }        
        public static void ConfigureSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(setup => {
                setup.SwaggerEndpoint("/swagger/OpenAPISpecification/swagger.json", "Desafío Técnico .NET API");
                setup.RoutePrefix = "OpenAPI";
            });
        }
    }
}
