using SocialApp.Domain.Common;
using SocialApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialApp.Domain.Events.SocialUserEvents
{
    public class NewUserRegisteredEvent : DomainEvent
    {
        public NewUserRegisteredEvent(SocialUser user)
        {
            User = user;
        }
        public SocialUser User { get; }
    }
}
