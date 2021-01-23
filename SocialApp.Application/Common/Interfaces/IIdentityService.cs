using SocialApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialApp.Application.Common.Responses;

namespace SocialApp.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<Response<string>> CreateAsync(string username, string email, string password);
        Task<Response<string>> FindByUsernameAsync(string username);
        Task<Response<string>> GetUserName(string id);
    }
}
