using FluentAssertions;
using SocialApp.Api;
using SocialApp.Application.User.Commands;
using SocialApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SocialApp.IntegrationTests.User.Commands
{
    public class CreateSocialUserCommandTests : IntegrationTest
    {
        public CreateSocialUserCommandTests(CustomWebApplicationFactory<Startup> factory) : base(factory) {}

        [Fact]
        public async Task Creating_WithValidInput_Succeeds()
        {
            var cmd = new CreateSocialUserCommand
            {
                Email = "testuser@social.app",
                Username = "testuser",
                Password = "P@ssw0rd"
            };

            var response = await SendAsync(cmd);

            response.Succeeded.Should().BeTrue();

            SocialUser socialUser = await FindAsync<SocialUser>(response.Data);

            socialUser.Should().NotBeNull();

            socialUser.Id.Should().Be(response.Data);
            socialUser.IdentityId.Value.Should().Be(response.Data.ToString());
        }

        [Theory]
        [InlineData("123456")]
        [InlineData("abcdefg")]
        public async Task Creating_WithWeakPassword_Fails(string password)
        {
            var cmd = new CreateSocialUserCommand
            {
                Email = "testuser@social.app",
                Username = "testuser",
                Password = password
            };

            var response = await SendAsync(cmd);

            response.Succeeded.Should().BeFalse();
            response.Error.Title.Should().Be("Registration Error");
        }

        [Theory]
        [InlineData("notanemail", "1234", "wwwwwww")]
        [InlineData("google", "5566", "xcxcxc")]
        public async Task Creating_WithInvalidInput_Fails(string email, string username, string password)
        {
            var cmd = new CreateSocialUserCommand
            {
                Email = email,
                Username = username,
                Password = password
            };

            var response = await SendAsync(cmd);

            response.Succeeded.Should().BeFalse();
            response.Error.Title.Should().Be("Registration Error");
        }
    }
}
