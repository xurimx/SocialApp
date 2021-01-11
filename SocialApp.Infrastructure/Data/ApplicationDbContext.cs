using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SocailApp.Application.Common.Interfaces;
using SocialApp.Domain.Common;
using SocialApp.Domain.Common.Interfaces;
using SocialApp.Domain.Entities;
using SocialApp.Infrastructure.Data.Configurations;
using SocialApp.Infrastructure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocialApp.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
    {
        private readonly ICurrentUserService currentUser;
        private readonly IDomainEventService eventService;

        public ApplicationDbContext(
                        DbContextOptions<ApplicationDbContext> options,
                        ICurrentUserService currentUser, IDomainEventService eventService)
                             : base(options)
        {
            this.currentUser = currentUser;
            this.eventService = eventService;
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)

        {

        }

        #region DbSets

        public DbSet<SocialUser> SocialUsers { get; set; }
        public DbSet<UserFriend> UserFriends { get; set; }

        public DbSet<FriendRequest> FriendRequests { get; set; }
        public DbSet<BlockedUser> BlockedUsers { get; set; }

        #endregion
        public override async Task<int> SaveChangesAsync(CancellationToken token = default)
        {
            foreach (EntityEntry<BaseEntity> entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = currentUser.IdentityId;
                        entry.Entity.Created = DateTime.UtcNow;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedBy = currentUser.IdentityId;
                        entry.Entity.LastModified = DateTime.UtcNow;
                        break;
                }
            }

            int result = await base.SaveChangesAsync(token);

            await DispatchEvents();

            return result;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(GetType().Assembly);

            base.OnModelCreating(builder);
        }



        #region Private Methods
        private async Task DispatchEvents()
        {
            List<IHasDomainEvent> eventEntities = ChangeTracker.Entries<IHasDomainEvent>().Select(x => x.Entity).ToList();
            foreach (var domainEntity in eventEntities)
            {
                foreach (var domainEvent in domainEntity.DomainEvents)
                {
                    if (!domainEvent.isPublished)
                    {
                        domainEvent.isPublished = true;
                        await eventService.Publish(domainEvent);
                    }
                }
                domainEntity.ClearDomainEvents();
            }
        }
        #endregion
    }
}
