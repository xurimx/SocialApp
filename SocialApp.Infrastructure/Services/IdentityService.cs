using Microsoft.AspNetCore.Identity;
using SocialApp.Application.Common.Interfaces;
using SocialApp.Application.Common.Responses;
using SocialApp.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialApp.Infrastructure.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> userManager;

        public IdentityService(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<Response<string>> CreateAsync(string username, string email, string password)
        {
            ApplicationUser user = new ApplicationUser
            {
                UserName = username,
                Email = email,
                EmailConfirmed = true
            };

            IdentityResult identityResult = await userManager.CreateAsync(user, password);

            if (identityResult.Succeeded)
            {                
                return Response<string>.Success(user.Id);
            }

            Dictionary<string, object> errors = identityResult.Errors.ToDictionary(x => x.Code, y => (object)y.Description);
            return Response<string>.Fail(Error.Create("Identity Error", "Could not create the user", errors));
        }

        public async Task<Response<string>> FindByUsernameAsync(string username)
        {
            ApplicationUser applicationUser = await userManager.FindByNameAsync(username);
            if (applicationUser != null)
            {
                return Response<string>.Success(applicationUser.Id);
            }
            return Response<string>.Fail("Identity Error", "Could not find a user with requested username.");
        }

        public async Task<Response<string>> GetUserName(string id)
        {
            ApplicationUser applicationUser = await userManager.FindByIdAsync(id);
            if (applicationUser != null)
            {
                return Response<string>.Success(applicationUser.UserName);
            }
            return Response<string>.Fail("Identity Error", "Could not find a user with requested id.");
        }
    }
}
