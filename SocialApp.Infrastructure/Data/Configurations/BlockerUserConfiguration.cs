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
    public class BlockerUserConfiguration : IEntityTypeConfiguration<BlockedUser>
    {
        public void Configure(EntityTypeBuilder<BlockedUser> builder)
        {
            builder.Property(x => x.Id);
            builder.HasOne(x => x.Blocker).WithMany(x => x.BlockedUsers);
            builder.HasOne(x => x.Blocked).WithOne().HasForeignKey<SocialUser>(x => x.Id);
         
        }
    }
}
