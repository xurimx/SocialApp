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
    public class FriendRequestConfiguration : IEntityTypeConfiguration<FriendRequest>
    {
        public void Configure(EntityTypeBuilder<FriendRequest> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id);

            builder.HasOne(x => x.Sender).WithMany(x => x.SentFriendRequests)
                .HasForeignKey("SenderId")               
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Receiver)
                .WithMany(x => x.PendingFriendRequests)
                .HasForeignKey("ReceiverId")
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Status).WithMany();
        }
    }
}
