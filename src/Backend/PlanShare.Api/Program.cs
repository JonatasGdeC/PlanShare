using System.Reflection;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi;
using PlanShare.Api.Converters;
using PlanShare.Api.Filters;
using PlanShare.Api.Middleware;
using PlanShare.Api.Token;
using PlanShare.Application;
using PlanShare.Domain.Security.Tokens;
using PlanShare.Infrastructure;
using PlanShare.Infrastructure.Extensions;
using PlanShare.Infrastructure.Migrations;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args: args);

builder.Services.AddControllers()
    .AddJsonOptions(configure: options => options.JsonSerializerOptions.Converters.Add(item: new StringConverter()));

builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(setupAction: config =>
{
    config.AddSecurityDefinition(name: "Bearer", securityScheme: new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = """
                      JWT Authorization header using the Bearer scheme.
                      Enter 'Bearer' [space] and then your token.
                      Example: 'Bearer 12345abcdef'
                      """,
        In = ParameterLocation.Header,
        Scheme = "Bearer",
        Type = SecuritySchemeType.ApiKey
    });

    config.AddSecurityRequirement(securityRequirement: document => new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference(referenceId: "Bearer", hostDocument: document),
            []
        }
    });
});

builder.Services.AddApplication();
builder.Services.AddInfrastructure(configuration: builder.Configuration);

builder.Services.AddRouting(configureOptions: options => options.LowercaseUrls = true);

builder.Services.AddMvc(setupAction: options => options.Filters.Add<ExceptionFilter>());

builder.Services.AddScoped<ITokenProvider, HttpContextTokenValue>();

builder.Services.AddHealthChecks();

builder.Services.AddHttpContextAccessor();

DateTime applicationStartedAt = DateTime.UtcNow;
WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<CultureMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapHealthChecks(pattern: "/health/check", options: new HealthCheckOptions
{
    AllowCachingResponses = false,
    ResultStatusCodes =
    {
        [key: HealthStatus.Healthy] = StatusCodes.Status200OK,
        [key: HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable,
    },
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";

        var response = new
        {
            version = Assembly.GetExecutingAssembly().GetName().Version?.ToString(),
            environment = app.Environment.EnvironmentName,
            databaseConnected = report.Status == HealthStatus.Healthy,
            applicationStartedAt = applicationStartedAt
        };

        await context.Response.WriteAsync(text: JsonSerializer.Serialize(value: response,
            options: new JsonSerializerOptions
            {
                WriteIndented = true
            }));
    }
});

app.MapControllers();

if (builder.Configuration.IsUnitTestEnviroment() == false)
{
    await MigrateDatabase();
}

app.Run();

async Task MigrateDatabase()
{
    await using AsyncServiceScope scope = app.Services.CreateAsyncScope();
    string stringConnection = builder.Configuration.ConnectionString();
    DataBaseMigration.Migrate(connectionString: stringConnection, serviceProvider: scope.ServiceProvider);
}