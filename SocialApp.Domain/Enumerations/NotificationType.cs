using SocialApp.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialApp.Domain.Enumerations
{
    public class NotificationType : Enumeration
    {
        public static NotificationType Other = new NotificationType(1, nameof(Other));
        public static NotificationType Reply = new NotificationType(2, nameof(Reply));
        public static NotificationType FriendRequest = new NotificationType(3, nameof(FriendRequest));
        public static NotificationType AcceptedFriendRequest = new NotificationType(4, nameof(AcceptedFriendRequest));

        public NotificationType(int id, string name) : base(id, name) { }
    }
}
