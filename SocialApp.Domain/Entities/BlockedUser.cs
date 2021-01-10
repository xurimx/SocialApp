using SocialApp.Domain.Common;
using SocialApp.Domain.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialApp.Domain.Entities
{
    public class BlockedUser : BaseEntity, IDomainEntity
    {
        public SocialUser Blocker { get; private set; }
        public SocialUser Blocked { get; private set; }
        private BlockedUser(SocialUser blocker, SocialUser blocked)
        {
            Blocker = blocker;
            Blocked = blocked;
        }

        protected BlockedUser(Guid id, SocialUser blocker, SocialUser blocked) 
            :this(blocker, blocked)
        { Id = id; }


        public static BlockedUser Create(SocialUser blocker, SocialUser blocked)
        {
            return new BlockedUser(Guid.NewGuid(), blocker, blocked);
        }
    }
}
