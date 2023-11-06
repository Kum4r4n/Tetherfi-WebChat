using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Common.Authentication
{
    public static class Extensions
    {
        public static IServiceCollection AddAuth(this IServiceCollection services, string key)
        {

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateActor = false,
                        ValidateLifetime = true,
                        SaveSigninToken = true
                    };
                    x.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            context.Request.Query.TryGetValue("token", out StringValues token);
                            context.Token = token;
                            return Task.CompletedTask;
                        },
                        OnAuthenticationFailed = context =>
                        {
                            var ex = context.Exception;
                            return Task.CompletedTask;
                        }
                    };

                });

            return services;
        }

        public static void UseAuth(this IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}
