using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using FluentMigrator.Runner;

namespace PlanShare.Infrastructure.Migrations;

public static class DataBaseMigration
{
    public static void Migrate(string connectionString, IServiceProvider serviceProvider)
    {
        NpgsqlConnectionStringBuilder connectionStringBuilder = new(connectionString: connectionString);
        string? databaseName = connectionStringBuilder.Database;
        connectionStringBuilder.Database = "postgres";
        using NpgsqlConnection dbConnection = new(connectionString: connectionStringBuilder.ConnectionString);
        DynamicParameters parameters = new();
        parameters.Add(name: "name", value: databaseName);

        IEnumerable<dynamic> records = dbConnection.Query(sql: "SELECT 1 FROM pg_database WHERE datname = @name", param: parameters);

        if (records.Any() == false)
        {
            dbConnection.Execute(sql: $"CREATE DATABASE \"{databaseName}\"");
        }
        
        MigrateDatabase(serviceProvider: serviceProvider);
    }

    private static void MigrateDatabase(IServiceProvider serviceProvider)
    {
        IMigrationRunner runner = serviceProvider.GetRequiredService<IMigrationRunner>();
        runner.ListMigrations();
        runner.MigrateUp();
    }
}