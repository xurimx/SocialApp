using SocialApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocailApp.Application.Common.Interfaces.Repositories
{
    public interface ISocialUserRepository
    {
        Task<SocialUser> Find(Guid id);
        Task Save(SocialUser socialUser);
    }
}
