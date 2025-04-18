using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BackPack.Dependency.Library.Cors
{
    public static class Startup
    {
        public const string CorsPolicy = nameof(CorsPolicy);

        public static IServiceCollection AddCorsPolicy(this IServiceCollection services, IConfiguration config)
        {
            var corsSettings = config.GetSection(nameof(CorsSettings)).Get<CorsSettings>();
            var origins = new List<string>();
            if (corsSettings!.Php is not null)
                origins.AddRange(corsSettings.Php.Split(';', StringSplitOptions.RemoveEmptyEntries));
            if (corsSettings!.React is not null)
                origins.AddRange(corsSettings.React.Split(';', StringSplitOptions.RemoveEmptyEntries));

            return services.AddCors(opt =>
                opt.AddPolicy(CorsPolicy, policy =>
                    policy.AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .WithOrigins(origins.ToArray())));
        }

        public static IApplicationBuilder UseCorsPolicy(this IApplicationBuilder app) =>
            app.UseCors(CorsPolicy);
    }
}
