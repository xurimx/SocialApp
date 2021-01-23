using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialApp.Infrastructure.Data
{
    public class SocialUserContextFactory : IDesignTimeDbContextFactory<SocialUserContext>
    {
        public SocialUserContext CreateDbContext(string[] args)
        {
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            //IConfiguration config = new ConfigurationBuilder()
            //    .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../SocialApp.Api"))
            //    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            //    .AddJsonFile($"appsettings.{environment}.json", optional: true)
            //    .AddEnvironmentVariables()
            //    .Build();

            var optionsBuilder = new DbContextOptionsBuilder<SocialUserContext>();

            //optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
            optionsBuilder.UseSqlServer("Data Source=localhost; Initial Catalog=socialappdb; Integrated Security=true");

            return new SocialUserContext(optionsBuilder.Options, null, null);
        }
    }
}
