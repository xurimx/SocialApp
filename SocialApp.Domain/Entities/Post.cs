using SocialApp.Domain.Common;
using SocialApp.Domain.Common.Interfaces;
using SocialApp.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialApp.Domain.Entities
{
    public class Post : BaseEntity, IDomainEntity
    {
        public SocialUser User { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public VisibilityType Visibility { get; set; }
    }
}
