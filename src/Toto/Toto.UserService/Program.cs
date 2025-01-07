using System.Reflection;
using MassTransit;
using Toto.Extensions.DI;
using Toto.Extensions.PostgreSQL;
using Toto.UserService.DataAccess.Context;
using Toto.UserService.DataAccess.Repositories;
using Toto.UserService.Domain.Interfaces;

namespace Toto.UserService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            
            var applyMigration = Environment.GetEnvironmentVariable("MIGRATION_KEY") == "initOrUpdateDb";
            host.Migrate<UserDbContext>(applyMigration);
            
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    var connectionString = hostContext.Configuration.GetConnectionString("DefaultConnection");
                    services.ConnectToDatabase<UserDbContext>(connectionString);

                    services.AddTransient<IUserRepository, UserRepository>();
                    services.AddTransient<IUserService, Application.UserService>();
                    
                    services.AddValidatedOption<QueryTimeLogOptions>(QueryTimeLogOptions.ConfigurationSectionName);
                    
                    services.AddMassTransit(x =>
                    {
                        x.SetKebabCaseEndpointNameFormatter();

                        // By default, sagas are in-memory, but should be changed to a durable
                        // saga repository.
                        x.SetInMemorySagaRepositoryProvider();

                        var entryAssembly = Assembly.GetEntryAssembly();

                        x.AddConsumers(entryAssembly);
                        x.AddSagaStateMachines(entryAssembly);
                        x.AddSagas(entryAssembly);
                        x.AddActivities(entryAssembly);

                        x.UsingRabbitMq((context,cfg) =>
                        {
                            cfg.Host("localhost", "/", h => {
                                h.Username("guest");
                                h.Password("guest");
                            });

                            cfg.ConfigureEndpoints(context);
                        });
                    });
                });
    }
}
