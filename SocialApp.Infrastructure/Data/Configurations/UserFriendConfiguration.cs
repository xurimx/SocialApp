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
    public class UserFriendConfiguration : IEntityTypeConfiguration<UserFriend>
    {
        public void Configure(EntityTypeBuilder<UserFriend> builder)
        {
            
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id);
            builder.Property(x => x.isFavorite);

            //builder.HasOne(x => x.User).WithMany(x => x.Friends)
            //    .HasForeignKey("UserId")               
            //    .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Friend)
                .WithMany()
                .HasForeignKey("FriendId");
        }
    }
}
