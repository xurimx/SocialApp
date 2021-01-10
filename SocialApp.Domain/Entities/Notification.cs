using SocialApp.Domain.Common;
using SocialApp.Domain.Common.Interfaces;
using SocialApp.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace SocialApp.Domain.Entities
{
    public class Notification : BaseEntity, IDomainEntity
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime TimeStamp { get; set; }
        public bool isRead { get; set; }
        public SocialUser Receiver { get; set; }
        public NotificationType Type { get; set; }
        public string Details { get; set; }
    }
}
