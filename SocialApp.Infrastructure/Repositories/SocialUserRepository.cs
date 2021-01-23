using Microsoft.EntityFrameworkCore;
using SocialApp.Application.Common.Interfaces.Repositories;
using SocialApp.Domain.Entities;
using SocialApp.Domain.Enumerations;
using SocialApp.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialApp.Infrastructure.Repositories
{
    public class SocialUserRepository : ISocialUserRepository
    {
        private readonly SocialUserContext context;

        public SocialUserRepository(SocialUserContext context)
        {
            this.context = context;
        }
        public async Task<SocialUser> Find(Guid id)
        {
            return await context.SocialUsers
                .Include(x => x.Friends)
                .Include(x => x.BlockedUsers)
                .Include(x => x.PendingFriendRequests.Where(p => p.Status == RequestStatus.Pending))
                .Include(x => x.SentFriendRequests.Where(p => p.Status == RequestStatus.Pending))
                .Where(x => x.Id == id).SingleOrDefaultAsync();
        }
    }
}
