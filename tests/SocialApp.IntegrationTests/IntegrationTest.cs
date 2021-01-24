using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Respawn;
using SocialApp.Api;
using SocialApp.Domain.Common.ValueObjects;
using SocialApp.Domain.Entities;
using SocialApp.Infrastructure.Data;
using SocialApp.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialApp.Application.Common.Interfaces;
using Xunit;
using Xunit.Abstractions;

namespace SocialApp.IntegrationTests
{
    public class IntegrationTest : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly CustomWebApplicationFactory<Startup> _factory;
        private IServiceProvider provider;
        protected IServiceScopeFactory _scopeFactory => 
                provider == null ? _factory.Services.GetRequiredService<IServiceScopeFactory>() 
                                 : provider.GetRequiredService<IServiceScopeFactory>();

        protected Guid currentUserId;

        private static Checkpoint _checkpoint;

        public IntegrationTest(CustomWebApplicationFactory<Startup> factory, ITestOutputHelper testOutput)
        {
            _factory = factory.SetOutPut(testOutput);

            EnsureCreated();
            _checkpoint = new Checkpoint
            {
                TablesToIgnore = new[] { "__EFMigrationsHistory" }
            };
            ResetState();
        }


        public async Task RunAsDefaultUserAsync()
        {   
            using var scope = _scopeFactory.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            ApplicationUser defaultUser = new ApplicationUser
            {
                UserName = "default",
                Email = "default@social.app"
            };

            IdentityResult identityResult = await userManager.CreateAsync(defaultUser, "P@ssw0rd");
            var id = defaultUser.Id;
            SocialUser socialUser = SocialUser.Create(
                                                Guid.Parse(id),
                                                Identity.Create(id),
                                                Username.Create("default"),
                                                EmailAddress.Create("default@social.app"),
                                                Image.Default,
                                                DateTime.Now);


            var newFactory = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    ICurrentUserService mockedUserService = Mock.Of<ICurrentUserService>(mock => mock.IdentityId == id && mock.SocialId == Guid.Parse(id));

                    services.AddScoped(_ => mockedUserService);
                });
            });

            provider = newFactory.Services;

            var newScope = _scopeFactory.CreateScope();
            var context = newScope.ServiceProvider.GetRequiredService<SocialUserContext>();
            context.SocialUsers.Add(socialUser);
            await context.SaveChangesAsync();
            currentUserId = Guid.Parse(id);
        }


        private void EnsureCreated()
        {
            using var scope = _scopeFactory.CreateScope();
            var ctx = scope.ServiceProvider.GetRequiredService<SocialUserContext>();
            ctx.Database.Migrate();
        }

        protected void ResetState()
        {
            _checkpoint.Reset(CustomWebApplicationFactory<Startup>.TestConnectionString);
        }

        public async Task<SocialUser> CreateAnotherUser(string username, string email)
        {
            using var scope = _scopeFactory.CreateScope();

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var context = scope.ServiceProvider.GetRequiredService<SocialUserContext>();

            ApplicationUser defaultUser = new ApplicationUser
            {
                UserName = username,
                Email = email
            };

            IdentityResult identityResult = await userManager.CreateAsync(defaultUser, "P@ssw0rd");

            var id = defaultUser.Id;
            SocialUser socialUser = SocialUser.Create(
                                            Guid.Parse(id),
                                            Identity.Create(id),
                                            Username.Create(username),
                                            EmailAddress.Create(email),
                                            Image.Default,
                                            DateTime.Now);

            context.SocialUsers.Add(socialUser);
            await context.SaveChangesAsync();
            return socialUser;

        }

        protected async Task AddAsync<TEntity>(TEntity entity) where TEntity : class
        {
            using var scope = _scopeFactory.CreateScope();
            var ctx = scope.ServiceProvider.GetService<SocialUserContext>();
            ctx.Add(entity);
            await ctx.SaveChangesAsync();
        }

        protected async Task<TEntity> FindAsync<TEntity>(Guid id) where TEntity : class
        {
            using var scope = _scopeFactory.CreateScope();
            var ctx = scope.ServiceProvider.GetService<SocialUserContext>();
            return await ctx.FindAsync<TEntity>(id);
        }

        protected async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            using var scope = _scopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetService<IMediator>();
            return await mediator.Send(request);
        }
    }
}
