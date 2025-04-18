using NetCore.AutoRegisterDi;

namespace BackPack.WebAPI.Validators
{
    public static class Startup
    {
        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.RegisterAssemblyPublicNonGenericClasses()
                .AsPublicImplementedInterfaces();

            return services;
        }
    }
}
