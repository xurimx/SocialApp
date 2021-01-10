using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialApp.Domain.Common.ValueObjects;
using SocialApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialApp.Infrastructure.Data.Configurations
{
    public class SocialUserConfiguration : IEntityTypeConfiguration<SocialUser>
    {
        public void Configure(EntityTypeBuilder<SocialUser> builder)
        {
            builder.Ignore(x => x.DomainEvents);

            builder.Property(x => x.Id);
            builder.Property(x => x.IdentityId).HasConversion(x => x.Value, x => Domain.Common.ValueObjects.Identity.Create(x)).IsRequired();
            builder.Property(x => x.UserName).HasConversion(x => x.Value, x => Username.Create(x)).IsRequired();
            builder.Property(x => x.Email).HasConversion(x => x.Value, x => EmailAddress.Create(x)).IsRequired();
            builder.Property(x => x.Image).HasConversion(x => x.Value, x => Image.Create(x));
            builder.Property(x => x.LastLogin);

            builder.OwnsOne(typeof(NotificationSettings), "NotificationSettings");

            builder.HasMany(x => x.Friends).WithOne(x => x.User)
                .OnDelete(DeleteBehavior.Cascade)
                .Metadata.PrincipalToDependent.SetPropertyAccessMode(PropertyAccessMode.Field);

            builder.HasMany(x => x.PendingFriendRequests).WithOne(x => x.Receiver)
                .OnDelete(DeleteBehavior.Cascade)
                .Metadata.PrincipalToDependent.SetPropertyAccessMode(PropertyAccessMode.Field);
                
            builder.HasMany(x => x.SentFriendRequests).WithOne(x => x.Sender)
                .OnDelete(DeleteBehavior.Cascade)
                .Metadata.PrincipalToDependent.SetPropertyAccessMode(PropertyAccessMode.Field);
            
            builder.HasMany(x => x.BlockedUsers).WithOne(x => x.Blocker).HasForeignKey(x => x.Id)
                .OnDelete(DeleteBehavior.Cascade)
                .Metadata.PrincipalToDependent.SetPropertyAccessMode(PropertyAccessMode.Field);               
        }
    }
}
