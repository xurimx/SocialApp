using SocialApp.Domain.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialApp.Domain.Common
{
    public class BaseEntity : IDomainEntity
    {
        public Guid Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime Created { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
    }
}
