//using SocailApp.Application.Common.Interfaces.Repositories;
//using SocialApp.Domain.Entities;
//using SocialApp.Domain.Enumerations;
//using SocialApp.Infrastructure.Data;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace SocialApp.Infrastructure.Repositories
//{
//    public class SocialUserRepository : ISocialUserRepository
//    {
//        private readonly ApplicationDbContext context;

//        public SocialUserRepository(ApplicationDbContext context)
//        {
//            this.context = context;
//        }
//        public async Task<SocialUser> Find(Guid id)
//        {
//            SocialUser user = await context.SocialUsers.FindAsync(id);

//            if (user == null)
//            {
//                return null;
//            }

//            await context.Entry(user).Collection(x => x.Friends).LoadAsync();
//            await context.Entry(user).Collection(x => x.BlockedUsers).LoadAsync();
//            await context.Entry(user).Collection(x => x.PendingFriendRequests.Where(x => x.Status == RequestStatus.Pending)).LoadAsync();
//            await context.Entry(user).Collection(x => x.SentFriendRequests.Where(x => x.Status == RequestStatus.Pending)).LoadAsync();

//            return user;
//        }

//        public Task Save(SocialUser socialUser)
//        {
//            context.SocialUsers.Attach(socialUser);
//            return Task.CompletedTask;
//        }
//    }
//}
