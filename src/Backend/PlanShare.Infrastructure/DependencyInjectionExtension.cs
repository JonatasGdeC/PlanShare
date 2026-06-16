using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlanShare.Domain.Repositories;
using PlanShare.Domain.Repositories.Association;
using PlanShare.Domain.Repositories.User;
using PlanShare.Domain.Repositories.WorkItem;
using PlanShare.Domain.Security.Cryptography;
using PlanShare.Domain.Security.Tokens;
using PlanShare.Domain.Services.LoggedUser;
using PlanShare.Infrastructure.DataAccess;
using PlanShare.Infrastructure.DataAccess.Repositories;
using PlanShare.Infrastructure.Extensions;
using PlanShare.Infrastructure.Security.Cryptography;
using PlanShare.Infrastructure.Security.Tokens.Access.Generator;
using PlanShare.Infrastructure.Security.Tokens.Access.Validator;
using PlanShare.Infrastructure.Services.LoggedUser;

namespace PlanShare.Infrastructure;
public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddRepositories(services: services);
        AddLoggedUser(services: services);
        AddTokenHandlers(services: services, configuration: configuration);
        AddPasswordEncripter(services: services);
        AddDbContext(services: services, configuration: configuration);
    }

    private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
    {
        string connectionString = configuration.ConnectionString();

        services.AddDbContext<PlanShareDbContext>(optionsAction: dbContextOptions =>
        {
            dbContextOptions.UseMySql(connectionString: connectionString, serverVersion: ServerVersion.AutoDetect(connectionString: connectionString));
        });
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
        services.AddScoped<IUserReadOnlyRepository, UserRepository>();
        services.AddScoped<IUserUpdateOnlyRepository, UserRepository>();

        services.AddScoped<IWorkItemWriteOnlyRepository, WorkItemRepository>();
        services.AddScoped<IWorkItemReadOnlyRepository, WorkItemRepository>();
        services.AddScoped<IWorkItemUpdateOnlyRepository, WorkItemRepository>();

        services.AddScoped<IPersonAssociationReadOnlyRepository, PersonAssociationRepository>();
    }

    private static void AddLoggedUser(IServiceCollection services) => services.AddScoped<ILoggedUser, LoggedUser>();

    private static void AddPasswordEncripter(IServiceCollection services)
    {
        services.AddScoped<IPasswordEncripter, BCryptNet>();
    }

    private static void AddTokenHandlers(IServiceCollection services, IConfiguration configuration)
    {
        uint expirationTimeMinutes = configuration.GetValue<uint>(key: "Settings:Jwt:ExpiresMinutes");
        string signingKey = configuration.GetValue<string>(key: "Settings:Jwt:SigningKey")!;

        services.AddScoped<IAccessTokenValidator>(implementationFactory: option => new JwtTokenValidator(signingKey: signingKey));
        services.AddScoped<IAccessTokenGenerator>(implementationFactory: option => new JwtTokenGenerator(expirationTimeMinutes: expirationTimeMinutes, signingKey: signingKey));
    }
}
