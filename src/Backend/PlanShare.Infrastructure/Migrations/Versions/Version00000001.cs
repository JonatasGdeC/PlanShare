using FluentMigrator;

namespace PlanShare.Infrastructure.Migrations.Versions;

[Migration(version: DatabaseVersions.TABLE_REGISTER_USER, description: "Create table to save the user's information")]
public class Version00000001 : ForwardOnlyMigration
{
    public override void Up()
    {
        Create.Table(tableName: "Users")
            .WithColumn(name: "Id").AsGuid().PrimaryKey().NotNullable()
            .WithColumn(name: "Active").AsBoolean().NotNullable().WithDefaultValue(value: true)
            .WithColumn(name: "CreatedOn").AsDateTime().NotNullable().WithDefaultValue(value: System.DateTime.UtcNow)
            .WithColumn(name: "Name").AsString(size: 255).NotNullable()
            .WithColumn(name: "Email").AsString(size: 255).NotNullable()
            .WithColumn(name: "Password").AsString(size: 2000).NotNullable();
    }
}