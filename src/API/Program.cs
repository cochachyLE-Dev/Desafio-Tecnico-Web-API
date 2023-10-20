using API.Infrastructure.Extension;
using API.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = builder.Configuration;

builder.WebHost.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.AddFile(configuration["logging:LoggingPath"]);
});

builder.Services.AddControllers().AddJsonOptions(o => 
{
    o.JsonSerializerOptions.PropertyNamingPolicy = null;    
});

builder.Services.AddDbContext(configuration);
builder.Services.AddSettings(configuration);
builder.Services.AddAuthenticationService(configuration);
builder.Services.AddSwaggerOpenAPI();
builder.Services.AddServiceLayer();
builder.Services.AddScopedServices();
builder.Services.AddTransientServices();

var app = builder.Build();

app.UseCors(options => options.WithOrigins("*").AllowAnyHeader().AllowAnyMethod());

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.ConfigureMiddleware();
app.ConfigureSwagger();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
