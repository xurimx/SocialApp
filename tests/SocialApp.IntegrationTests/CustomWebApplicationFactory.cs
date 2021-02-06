using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SocialApp.Infrastructure.Data;
using SocialApp.IntegrationTests.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace SocialApp.IntegrationTests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        public IServiceScopeFactory _scopeFactory;
        // TODO: Get from appsettings.json 
        public static string TestConnectionString = "Data Source=localhost; Initial Catalog=test_socialappdb; Integrated Security=true";

        public ITestOutputHelper Output { get; set; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.Services.AddSingleton<ILoggerProvider>(serviceProvider => new XUnitLoggerProvider(Output));
            });

            builder.ConfigureServices(services =>
            {
                ServiceDescriptor socialContextDescriptor = services.SingleOrDefault(x => x.ServiceType == typeof(DbContextOptions<SocialUserContext>));
                services.Remove(socialContextDescriptor);

                services.AddDbContext<SocialUserContext>(options =>
                {
                    options.UseSqlServer(TestConnectionString);
                });

                var sp = services.BuildServiceProvider();

                _scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
            });
            base.ConfigureWebHost(builder);
        }

        public CustomWebApplicationFactory<TStartup> SetOutPut(ITestOutputHelper output)
        {
            Output = output;
            return this;
        }
    }
}
