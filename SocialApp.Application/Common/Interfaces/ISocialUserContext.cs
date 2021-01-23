using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SocialApp.Domain.Common.Interfaces;
using SocialApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocialApp.Application.Common.Interfaces
{
    public interface ISocialUserContext
    {
        DbSet<SocialUser> SocialUsers { get; set; }
        DbSet<FriendRequest> FriendRequests { get; set; }
        DbSet<BlockedUser> BlockedUsers { get; set; }
        Task<int> SaveChangesAsync(CancellationToken token);
    }
}
