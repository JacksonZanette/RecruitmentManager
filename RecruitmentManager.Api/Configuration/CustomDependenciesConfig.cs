using RecruitmentManager.Domain.Configuration;
using RecruitmentManager.Domain.Interfaces.Repositories;
using RecruitmentManager.Domain.Interfaces.Services;
using RecruitmentManager.Domain.Services;
using RecruitmentManager.Infra.Database;
using RecruitmentManager.Infra.Repositories;

namespace RecruitmentManager.Api.Configuration
{
    public static class CustomDependenciesConfig
    {
        public static void AddCustomDependencies(this IServiceCollection services, ConfigurationManager configurationManager)
        {
            services.AddAutoMapper(typeof(AutoMapperConfiguration));

            services.AddSingleton(new DatabaseConfig { Name = configurationManager["DatabaseName"] });
            services.AddSingleton<DatabaseSetup>();

            services.AddScoped<ICandidatesService, CandidatesService>();
            services.AddScoped<ICandidatesRepository, CandidatesRepository>();
        }
    }
}