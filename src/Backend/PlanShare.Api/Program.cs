using Microsoft.OpenApi;
using PlanShare.Api.Filters;
using PlanShare.Api.Middleware;
using PlanShare.Api.Token;
using PlanShare.Application;
using PlanShare.Domain.Security.Tokens;
using PlanShare.Infrastructure;
using PlanShare.Infrastructure.DataAccess.Migrations;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args: args);

builder.Services.AddControllers();

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

builder.Services.AddHttpContextAccessor();

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

app.MapControllers();

await MigrateDatabase();

app.Run();

async Task MigrateDatabase()  
{  
    await using AsyncServiceScope scope = app.Services.CreateAsyncScope();  
    await DataBaseMigration.MigrateDatabase(serviceProvider: scope.ServiceProvider);  
}