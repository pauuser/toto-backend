using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using MassTransit;
using Toto.AuthService.Application;
using Toto.AuthService.DataAccess.Context;
using Toto.AuthService.DataAccess.Repositories;
using Toto.AuthService.Domain.Configuration;
using Toto.AuthService.Domain.Interfaces;
using Toto.AuthService.Services;
using Toto.Extensions.DI;
using Toto.Extensions.PostgreSQL;

namespace Toto.AuthService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    var connectionString = hostContext.Configuration.GetConnectionString("DefaultConnection");
                    services.ConnectToDatabase<AuthDbContext>(connectionString);

                    services.AddTransient<ITokenRepository, TokenRepository>();
                    
                    services.AddValidatedOption<JwtTokenConfiguration>(JwtTokenConfiguration.ConfigurationSectionName);
                    services.AddValidatedOption<QueryTimeLogOptions>(QueryTimeLogOptions.ConfigurationSectionName);
                    
                    services.AddTransient<ILoginService, LoginService>();
                    services.AddTransient<ITokenService, TokenService>();
                    
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
