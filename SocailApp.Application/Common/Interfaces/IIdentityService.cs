using SocialApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocailApp.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<string> CreateAsync(string username, string email, string password);
        Task<string> FindByUsernameAsync(string username);
        Task<string> GetUserName(string id);
    }
}
