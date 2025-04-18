using BackPack.Tenant.WebAPI.Configurations;
using BackPack.Dependency.Library.Cors;
using BackPack.Dependency.Library.Swagger;
using BackPack.Tenant.Library.Services;
using BackPack.Tenant.WebAPI.Validators;
using MassTransit;
using BackPack.Tenant.Library.Consumers;
using BackPack.Dependency.Library.JWT;
using Swashbuckle.AspNetCore.SwaggerUI;
using BackPack.Tenant.Library.Middleware;
using BackPack.Dependency.Library.Helpers;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.AddConfigurations();

Aes256Helper aes256Helper = new(builder.Configuration);

// Serilog
builder.Logging.ClearProviders();
var _connectionString = aes256Helper.Aes256Decryption(builder.Configuration.GetSection("DatabaseSettings").GetSection("LogConnectionString").Value!);
var _tableName = builder.Configuration.GetSection("CommonSettings").GetSection("SerilogTableName").Value;
var _schemaName = builder.Configuration.GetSection("CommonSettings").GetSection("SerilogSchemaName").Value;

Log.Logger = new LoggerConfiguration()
    .WriteTo
    .PostgreSQL(connectionString: _connectionString, tableName: _tableName, schemaName: _schemaName, needAutoCreateTable: true)
    .CreateLogger();

builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Swagger
builder.Services.AddSwagger();

// JWT
builder.Services.AddJWT(builder.Configuration);

// Fluent validators
builder.Services.AddValidators();

// Services
builder.Services.AddServices();

// Message broker
builder.Services.AddMassTransit(busConfiguration =>
{
    busConfiguration.SetKebabCaseEndpointNameFormatter();

    busConfiguration.AddConsumer<GetTenantConsumer>();
    busConfiguration.AddConsumer<GetTenantByIDConsumer>();

    busConfiguration.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(new Uri(builder.Configuration.GetSection("MessageBrokerSettings").GetSection("Host").Value!), host =>
        {
            host.Username(aes256Helper.Aes256Decryption(builder.Configuration.GetSection("MessageBrokerSettings").GetSection("Username").Value!));
            host.Password(aes256Helper.Aes256Decryption(builder.Configuration.GetSection("MessageBrokerSettings").GetSection("Password").Value!));
        });

        configurator.ConfigureEndpoints(context);
    });
});
    


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.UseSwagger();
    app.UseSwaggerUI(option =>
    {
        option.DefaultModelsExpandDepth(-1);
        option.DocExpansion(DocExpansion.None);
    });
}

// Middleware
app.UseMiddleware<JwtMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();

