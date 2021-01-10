using SocialApp.Domain.Common;
using SocialApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialApp.Domain.Events.SocialUserEvents
{
    public class FriendHasBeenRemovedEvent : DomainEvent
    {
        public FriendHasBeenRemovedEvent(UserFriend userFriend)
        {
            UserFriend = userFriend;
        }
        public UserFriend UserFriend { get; }
    }
}
