using BackPack.Library.Middleware;
using BackPack.Library.Services;
using BackPack.WebAPI.Configurations;
using MassTransit;
using BackPack.Dependency.Library.Swagger;
using BackPack.WebAPI.Validators;
using BackPack.Library.Consumers;
using BackPack.Dependency.Library.JWT;
using Swashbuckle.AspNetCore.SwaggerUI;
using BackPack.Dependency.Library.Helpers;
using Serilog;
using Microsoft.AspNetCore.Authorization;

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

// Cors


builder.Services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
// Add services to the container .

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

    busConfiguration.AddConsumer<CreateDomainConsumer>();
    busConfiguration.AddConsumer<UpdateDomainConsumer>();
    busConfiguration.AddConsumer<DeleteDomainConsumer>();
    busConfiguration.AddConsumer<GetDomainConsumer>();
    busConfiguration.AddConsumer<GetSuperAdminDashboardConsumer>();
    busConfiguration.AddConsumer<PublicCourseCapsuleByDomainConsumer>();
    busConfiguration.AddConsumer<GetAllSubjectsByDomainConsumer>();
    busConfiguration.AddConsumer<GetCourseCapsuleByCapsuleConsumer>();
    busConfiguration.AddConsumer<GetAllCoursesByDomainConsumer>();
    busConfiguration.AddConsumer<GetAllTeachersByClassConsumer>();
    busConfiguration.AddConsumer<GetAllLPCourseLicensesConsumer>();
    busConfiguration.AddConsumer<CreateCourseCapsuleLicenseConsumer>();

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

builder.Services.AddScoped<IAuthorizationHandler, RoleAuthorizationHandler>();

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

app.UseStatusCodePagesWithRedirects("/api/Error/{0}");

app.UseMiddleware<JwtMiddleware>();

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
