namespace BackPack.APIGateway.Configurations
{
    internal static class Startup
    {
        internal static IHostBuilder AddConfigurations(this IHostBuilder host)
        {
            host.ConfigureAppConfiguration((context, config) =>
            {
                const string configurationsDirectory = "Configurations";
                var env = context.HostingEnvironment;
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/Cors.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/Cors.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/Ocelot.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/JWT.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/JWT.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/AES256.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/AES256.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/Database.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/Database.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/Common.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"{configurationsDirectory}/Common.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)

                    .AddEnvironmentVariables();
            });

            return host;
        }
    }
}
