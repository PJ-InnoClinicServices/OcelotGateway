using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace OcelotGateway.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var environment = configuration.GetValue<string>("ASPNETCORE_ENVIRONMENT");

            string authority;
            string audience;
            string signingKey;

            if (environment == "Container")
            {
                authority = Environment.GetEnvironmentVariable("JWT_AUTHORITY");
                audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");
                signingKey = Environment.GetEnvironmentVariable("JWT_SigningKey");
            }
            else
            {
                var jwtSettings = configuration.GetSection("JwtSettings");
                authority = jwtSettings.GetValue<string>("Authority");
                audience = jwtSettings.GetValue<string>("Audience");
                signingKey = jwtSettings.GetValue<string>("SigningKey");
            }

            var keyBytes = Encoding.UTF8.GetBytes(signingKey);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = authority;
                    options.Audience = audience;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = authority,
                        ValidAudience = audience,
                        IssuerSigningKey = new SymmetricSecurityKey(keyBytes)
                    };
                });

            services.AddAuthorization();

            return services;
        }
    }
}
