using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Toto.Extensions.PostgreSQL.Exceptions;

namespace Toto.Extensions.PostgreSQL;

public static class MigrationExtensions
{
    public static IServiceCollection ConnectToDatabase<TContext>(this IServiceCollection serviceCollection,
        string? connectionString) where TContext : DbContext
    {
        serviceCollection.AddDbContext<TContext>(options => options.UseNpgsql(connectionString));

        return serviceCollection;
    }

    public static IHost Migrate<TContext>(this IHost host, bool initOrUpdateDb) where TContext : DbContext
    {
        using var scope = host.Services.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<TContext>();
        if (initOrUpdateDb)
        {
            try
            {
                context.Database.Migrate();
            }
            catch (Exception ex)
            {
                throw new MigrateException(ex);
            }
        }
        else
        {
            var databaseExists = context.Database.CanConnect();
            if (!databaseExists)
                throw new DatabaseNotFoundException();
        }

        return host;
    }
}