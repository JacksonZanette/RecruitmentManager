using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using RecruitmentManager.Api.Services;
using RecruitmentManager.Domain.Configuration;
using RecruitmentManager.Domain.Interfaces.Repositories;
using RecruitmentManager.Domain.Interfaces.Services;
using RecruitmentManager.Domain.Services;
using RecruitmentManager.Infra.Database;
using RecruitmentManager.Infra.Repositories;
using System.Text;

namespace RecruitmentManager.Api.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void AddCustomDependencies(this IServiceCollection services, ConfigurationManager configurationManager)
        {
            services.AddAutoMapper(typeof(AutoMapperConfiguration));

            services.AddSingleton(new DatabaseConfig { Name = configurationManager["DatabaseName"] });
            services.AddSingleton<DatabaseSetup>();

            services.AddScoped<ICandidatesService, CandidatesService>();
            services.AddScoped<ICandidatesRepository, CandidatesRepository>();

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IUsersRepository, UsersRepository>();
        }

        public static void AddCustomAuthentication(this IServiceCollection services)
        {
            var key = Encoding.ASCII.GetBytes(ApiSettings.JWT_SECRET);

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
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }
    }
}