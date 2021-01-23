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

namespace SocialApp.IntegrationTests.User.Commands
{
    public class SendFriendRequestCommandTests : IntegrationTest
    {
        public SendFriendRequestCommandTests(CustomWebApplicationFactory<Startup> factory) : base(factory) {}

        [Fact]
        public async Task User_SendingFriendRequest_Succeeds()
        {
            await RunAsDefaultUserAsync();
            SocialUser friend = await CreateAnotherUser("friendUsername", "friend@social.app");

            var cmd = new SendFriendRequestCommand { UserId = friend.Id };

            var response = await SendAsync(cmd);
           
            Assert.True(response.Succeeded);
            //ResetState();
        }
    }
}
