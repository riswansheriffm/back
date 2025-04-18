using Microsoft.Extensions.DependencyInjection;
using NetCore.AutoRegisterDi;

namespace BackPack.Tenant.Library.Services
{
    public static class Startup
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.RegisterAssemblyPublicNonGenericClasses()
                .AsPublicImplementedInterfaces();

            return services;
        }
    }
}
