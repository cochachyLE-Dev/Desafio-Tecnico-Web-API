using API.Infrastructure.Extension;
using API.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(o => {
                o.JsonSerializerOptions.PropertyNamingPolicy = null;
                //o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
            });            
            services.AddDbContext(Configuration);
            services.AddSettings(Configuration);
            services.AddAuthenticationService(Configuration);
            services.AddSwaggerOpenAPI();
            services.AddServiceLayer();
            services.AddScopedServices();
            services.AddTransientServices();            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();                
            }

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
        }
    }
}
