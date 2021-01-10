using SocialApp.Domain.Common;
using System.Collections.Generic;

namespace SocialApp.Domain.Common.ValueObjects
{
    public class NotificationSettings : ValueObject
    {
        public static NotificationSettings Default = new NotificationSettings(true, true, true, true);
        private NotificationSettings(bool replies, bool friendRequests, bool reactions, bool newMessages)
        {
            Replies = replies;
            FriendRequests = friendRequests;
            Reactions = reactions;
            NewMessages = newMessages;
        }

        #region Properties
        public bool Replies { get; private set; }
        public bool FriendRequests { get; private set; } 
        public bool Reactions { get; private set; } 
        public bool NewMessages { get; private set; }
        #endregion

        #region Methods
        public void Update(bool replies, bool friendRequests, bool reactions, bool newMessages)
        {
            Replies = replies;
            FriendRequests = friendRequests;
            Reactions = reactions;
            NewMessages = newMessages;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Replies;
            yield return FriendRequests;
            yield return Reactions;
            yield return NewMessages;
        }

        #endregion

        #region Factories

        public NotificationSettings Create(bool replies, bool friendRequests, bool reactions, bool newMessages)
        {
            return new NotificationSettings(replies, friendRequests, reactions, newMessages);
        }

        #endregion
    }
}
