using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using SocialApp.Api;
using SocialApp.Application.Common.Interfaces;
using SocialApp.Application.Common.Interfaces.Repositories;
using SocialApp.Application.User.Queries;
using SocialApp.Domain.Entities;
using SocialApp.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SocialApp.IntegrationTests.User.Queries
{
    public class GetFriendsQueryTests : IntegrationTest
    {
        public GetFriendsQueryTests(CustomWebApplicationFactory<Startup> factory, ITestOutputHelper testOutput) : base(factory, testOutput)
        {
        }

        [Fact]
        public async Task Query_WithNoFriends_ReturnsEmptyEnumerable()
        {
            await RunAsDefaultUserAsync();

            var query = new GetFriendsQuery();
            var friends = await SendAsync(query);

            friends.Should().BeEmpty();
            ResetState();
        }

        [Fact]
        public async Task Query_WithFriends_ReturnsFilledEnumerable()
        {
            await RunAsDefaultUserAsync();

            SocialUser friendOne = await CreateAnotherUser("testFriend", "test@mail.com");
            SocialUser friendTwo = await CreateAnotherUser("testFriendTwo", "testTwo@mail.com");

            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<SocialUserContext>();
            var socialUserRepository = scope.ServiceProvider.GetRequiredService<ISocialUserRepository>();
            SocialUser currentUserAggregate = await socialUserRepository.Find(currentUserId);

            context.AttachRange(friendOne, friendTwo);
            await context.AddRangeAsync(
                            UserFriend.Create(currentUserAggregate, friendOne),
                            UserFriend.Create(currentUserAggregate, friendTwo),
                            UserFriend.Create(friendOne, currentUserAggregate),
                            UserFriend.Create(friendTwo, currentUserAggregate));

            await context.SaveChangesAsync();

            var query = new GetFriendsQuery();
            var friends = await SendAsync(query);

            friends.Should().NotBeEmpty();
            friends.Any(x => x.Id == friendOne.Id).Should().BeTrue();
            friends.Any(x => x.Id == friendTwo.Id).Should().BeTrue();
            ResetState();
        }
    }
}
