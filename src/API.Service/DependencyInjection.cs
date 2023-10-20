using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using API.Domain.Shared;
using Microsoft.AspNetCore.Http;
using API.Domain.Settings;
using MediatR;
using API.Service.Implementation;

namespace API.Service
{
    public static class DependencyInjection
    {
        public static void AddServiceLayer(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddMediatR(Assembly.GetExecutingAssembly());
            serviceCollection.AddSingleton<MemoryCacheService>();            
        }        
        public static void AddAuthenticationService(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o => {
                o.RequireHttpsMetadata = false;
                o.SaveToken = false;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = configuration["JWTSettings:Issuer"],
                    ValidAudience = configuration["JWTSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSettings:Key"]!))
                };
                o.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception is SecurityTokenExpiredException)
                        {
                            context.Response.StatusCode = 401;
                            context.Response.ContentType = "application/json";
                            context.Response.Headers.Add("Token-Expired", "true");
                            var result = JsonSerializer.Serialize(Response.Fail(StatusCode.PermissionDenied, "Token Expired"));
                            return context.Response.WriteAsync(result);
                        }
                        else
                        {
                            context.NoResult();
                            context.Response.StatusCode = 500;
                            context.Response.ContentType = "text/plain";
                            var result = JsonSerializer.Serialize(Response.Fail(StatusCode.InvalidArgument, context.Exception.ToString()));
                            return context.Response.WriteAsync(result);
                        }
                    },
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        var result = JsonSerializer.Serialize(Response.Fail(StatusCode.Unauthenticated, "You are not Authorized"));
                        return context.Response.WriteAsync(result);
                    },
                    OnForbidden = context =>
                    {
                        context.Response.StatusCode = 403;
                        context.Response.ContentType = "application/json";
                        var result = JsonSerializer.Serialize(Response.Fail(StatusCode.PermissionDenied, "You are not authorized to access this resource"));
                        return context.Response.WriteAsync(result);
                    }
                };
            });
        }
        public static void AddSettings(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.Configure<JWTSettings>(configuration.GetSection("JWTSettings"));
        }      
    }
}
