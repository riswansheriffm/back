using Ocelot.Middleware;
using Ocelot.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerUI;
using BackPack.APIGateway.Cors;
using BackPack.APIGateway.Configurations;
using Ocelot.Provider.Polly;
using Serilog;
using BackPack.Dependency.Library.Helpers;
using BackPack.APIGateway.Middleware;
using Ocelot.Cache.CacheManager;
using Ocelot.Requester;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.AddConfigurations();

Aes256Helper aes256Helper = new(builder.Configuration);

// Serilog
builder.Logging.ClearProviders();

var connectionString = aes256Helper.Aes256Decryption(builder.Configuration.GetSection("DatabaseSettings").GetSection("LogConnectionString").Value!);
var tableName = builder.Configuration.GetSection("CommonSettings").GetSection("SerilogTableName").Value;
var schemaName = builder.Configuration.GetSection("CommonSettings").GetSection("SerilogSchemaName").Value;

Log.Logger = new LoggerConfiguration()
    .WriteTo
    .PostgreSQL(connectionString: connectionString, tableName: tableName, schemaName: schemaName, needAutoCreateTable: true)
    .CreateLogger();

// Cors
builder.Services.AddCorsPolicy(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddOcelot()
    .AddPolly()
    .AddCacheManager(x => x.WithDictionaryHandle());

builder.Services.AddLogging();

builder.Services.AddSwaggerForOcelot(builder.Configuration);

WebApplication app = builder.Build();

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.UseSwaggerForOcelotUI(option =>
    {
        option.PathToSwaggerGenerator = "/swagger/docs";
    }, uiOption =>
    {
        uiOption.DefaultModelsExpandDepth(-1);
        uiOption.DocExpansion(DocExpansion.None);
    });
}

app.UseMiddleware<JwtMiddleware>();

app.UseMiddleware<RequestResponseLoggingMiddleware>();

app.MapControllers();

await app.UseOcelot();

await app.RunAsync();
