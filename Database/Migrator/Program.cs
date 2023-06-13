// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;
using Dapper;  
 
Console.WriteLine("Hello, World!");

Migrator.initConfig();
Migrator.initDatabase();
Migrator.migrate();

static class MigratorConfig
{
    public static string? DatabaseServer { get; set; }
    public static string? DatabaseName { get; set; }
    public static string? DatabaseUserId { get; set; }
    public static string? DatabasePassword { get; set; } 
}

static class Migrator
{
    public static void initConfig()
    {
        var builder = new ConfigurationBuilder()
      .AddJsonFile($"appsettings.json", true, true);

        var config = builder.Build();

        MigratorConfig.DatabaseServer = config["Database_Server"];
        MigratorConfig.DatabaseName = config["Database_Name"];
        MigratorConfig.DatabaseUserId = config["Database_UserId"];
        MigratorConfig.DatabasePassword = config["Database_Password"];
    }

    public static NpgsqlConnection getConnection()
    {
        var connectionString = $"Host={MigratorConfig.DatabaseServer}; Database={MigratorConfig.DatabaseName}; Username={MigratorConfig.DatabaseUserId}; Password={MigratorConfig.DatabasePassword};";
        var connection = new NpgsqlConnection(connectionString);

        return connection;
    }

    public static void initDatabase()
    {
        var connection = getConnection();

        var sqlDbCount = $"SELECT COUNT(*) FROM pg_database WHERE datname = '{MigratorConfig.DatabaseName}';";
        var dbCount = connection.ExecuteScalar<int>(sqlDbCount);

        if (dbCount == 0)
        {
            var sql = $"CREATE DATABASE \"{MigratorConfig.DatabaseName}\"";
            connection.Execute(sql);
        }
    }

    public static void migrate()
    {
        var connection = getConnection();

        List<string> sqlStrings = new List<string>();

        foreach (string file in Directory.EnumerateFiles("./Database", "*.sql"))
        {
            sqlStrings.Add(File.ReadAllText(file));
        }

        foreach(string sqlCommand in sqlStrings)
        {
            connection.Execute(sqlCommand);
        }
    }
}
 






  





