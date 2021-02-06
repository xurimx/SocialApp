using Microsoft.Extensions.DependencyInjection;
using SocialApp.Api;
using SocialApp.Domain.Entities;
using SocialApp.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialApp.Application.Common.Responses;
using SocialApp.Application.User.Commands;
using Xunit;
using FluentAssertions;
using SocialApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace SocialApp.IntegrationTests.User.Commands
{
    public class SendFriendRequestCommandTests : IntegrationTest
    {
        public SendFriendRequestCommandTests(CustomWebApplicationFactory<Startup> factory, ITestOutputHelper outputHelper) : base(factory, outputHelper) {}

        [Fact]
        public async Task User_SendingFriendRequest_Succeeds()
        {
            await RunAsDefaultUserAsync();
            SocialUser friend = await CreateAnotherUser("friendUsername", "friend@social.app");

            var cmd = new SendFriendRequestCommand { UserId = friend.Id };

            var response = await SendAsync(cmd);

            using var scope = _scopeFactory.CreateScope();
            var ctx = scope.ServiceProvider.GetRequiredService<SocialUserContext>();

            ctx.SocialUsers.Attach(friend);
            await ctx.Entry(friend)
                .Collection(x => x.PendingFriendRequests)
                .Query()
                .Include(y => y.Sender)
                .Include(y => y.Receiver)
                .ToListAsync();


            bool receivedRequest = friend.PendingFriendRequests.Any(x => x.Sender.Id == currentUserId);

            receivedRequest.Should().BeTrue();
            response.Succeeded.Should().BeTrue();
            ResetState();
        }

        [Fact]
        public async Task User_SendingRandomGuid_Fails()
        {
            await RunAsDefaultUserAsync();
            var cmd = new SendFriendRequestCommand { UserId = Guid.NewGuid() };
            var response = await SendAsync(cmd);

            response.Succeeded.Should().BeFalse();
            ResetState();
        }
    }
}
