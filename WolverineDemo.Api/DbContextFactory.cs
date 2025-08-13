using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using WolverineDemo.Api;

namespace Payments.Data.Migrations;

public class DbContextFactory : IDesignTimeDbContextFactory<DataContext>
{
    public DataContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddUserSecrets<DbContextFactory>()
            .AddCommandLine(args)
            .Build();

        var connectionString = configuration.GetConnectionString("sql");

        var connection = new SqlConnection(connectionString);
        Console.WriteLine(connectionString);

        var throwException = false;
        try
        {
            var modifiedConnectionString = new SqlConnectionStringBuilder(connectionString);
            if (string.IsNullOrEmpty(modifiedConnectionString.UserID) || string.IsNullOrEmpty(modifiedConnectionString.Password))
            {
                Console.WriteLine($"Using access token");
                throwException = true;
                var credential = new Azure.Identity.DefaultAzureCredential();
                var token = credential.GetToken(new Azure.Core.TokenRequestContext(["https://database.windows.net/.default"]));
                modifiedConnectionString.Authentication = SqlAuthenticationMethod.NotSpecified;

                connection = new SqlConnection(modifiedConnectionString.ConnectionString)
                {
                    AccessToken = token.Token
                };
            }
            else
            {
                Console.WriteLine("Using username and password");
            }
        }
        catch (Exception)
        {
            if (throwException)
            {
                throw;
            }
        }

        var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
        optionsBuilder.UseSqlServer(
            connection,
            x => x.MigrationsAssembly(typeof(DbContextFactory).Assembly.FullName)
        );

        return new DataContext(optionsBuilder.Options);
    }
}
