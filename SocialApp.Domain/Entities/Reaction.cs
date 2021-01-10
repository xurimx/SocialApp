using SocialApp.Domain.Common;
using SocialApp.Domain.Common.Interfaces;
using SocialApp.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialApp.Domain.Entities
{
    public class Reaction : BaseEntity, IDomainEntity
    {
        public Post Post { get; set; }
        public SocialUser User { get; set; }
        public ReactionType Type { get; set; }
    }
}
