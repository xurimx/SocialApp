using SocialApp.Domain.Common;
using SocialApp.Domain.Common.Interfaces;
using SocialApp.Domain.Common.ValueObjects;
using SocialApp.Domain.Events.SocialUserEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialApp.Domain.Entities
{
    public class SocialUser : BaseEntity, IAggregateRoot
    {
        #region Domain Properties
        public List<DomainEvent> DomainEvents { get; }
        #endregion

        #region Constructors
        protected SocialUser(Identity identityId, Username userName, EmailAddress email, Image image, DateTime lastLogin)
        {
            IdentityId = identityId;
            UserName = userName;
            Email = email;
            Image = image;
            LastLogin = lastLogin;
        }

        private SocialUser(Guid id, Identity identityId, Username userName, EmailAddress email, Image image, DateTime lastLogin)
            : this(identityId, userName, email, image, lastLogin)
        {
            Id = id;
        }


        #endregion

        #region Properties
        public Identity IdentityId { get; private set; }
        public Username UserName { get; private set; }
        public EmailAddress Email { get; private set; }
        public Image Image { get; private set; }
        public DateTime LastLogin { get; private set; }
        public NotificationSettings NotificationSettings { get; private set; }

        private readonly List<UserFriend> _friends = new List<UserFriend>();

        private readonly List<FriendRequest> _pendingFriendRequests = new List<FriendRequest>();
        private readonly List<FriendRequest> _sentFriendRequests = new List<FriendRequest>();
        private readonly List<BlockedUser> _blockedUsers = new List<BlockedUser>();

        public virtual IReadOnlyList<UserFriend> Friends => _friends;
        public virtual IReadOnlyList<FriendRequest> PendingFriendRequests => _pendingFriendRequests;
        public virtual IReadOnlyList<FriendRequest> SentFriendRequests => _sentFriendRequests;
        public virtual IReadOnlyList<BlockedUser> BlockedUsers => _blockedUsers;
        #endregion

        #region Factories

        public static SocialUser Create(Guid id, Identity identityId, Username userName, EmailAddress email, Image image, DateTime lastLogin)
        {
            SocialUser socialUser = new SocialUser(id, identityId, userName, email, image, lastLogin);
            socialUser.NotificationSettings = NotificationSettings.Default;

            socialUser.DomainEvents.Add(new NewUserRegisteredEvent(socialUser));
            return socialUser;
        }

        #endregion

        #region Methods 

        public void SendFriendRequest(SocialUser friend)
        {
            FriendRequest request = FriendRequest.Create(this, friend);
            _sentFriendRequests.Add(request);
            DomainEvents.Add(new NewFriendRequestEvent(request));
        }

        public void AcceptFriendRequest(FriendRequest request)
        {
            request.Accept();
            _pendingFriendRequests.Remove(request);

            _friends.Add(UserFriend.Create(this, request.Receiver));
            _friends.Add(UserFriend.Create(request.Receiver, this));

            DomainEvents.Add(new FriendRequestAcceptedEvent(request));
        }

        public void RejectFriendRequest(FriendRequest friendRequest)
        {
            friendRequest.Reject();
            _pendingFriendRequests.Remove(friendRequest);
            DomainEvents.Add(new FriendRequestRejectedEvent(friendRequest));

        }

        public void BlockUser(SocialUser userToBlock)
        {
            if (!_blockedUsers.Any(x => x.Blocked == userToBlock))
            {
                BlockedUser blocked = BlockedUser.Create(this, userToBlock);
                _blockedUsers.Add(blocked);
                DomainEvents.Add(new UserHasBeenBlockedEvent(blocked));
            }
        }

        public void RemoveFriend(UserFriend userFriend)
        {
            if (_friends.Contains(userFriend))
            {
                _friends.Remove(userFriend);
                DomainEvents.Add(new FriendHasBeenRemovedEvent(userFriend));
            }
        }

        public void RemoveFriend(SocialUser friend)
        {
            UserFriend userFriend = _friends.FirstOrDefault(x => x.Id == friend.Id);
            if (userFriend != null)
            {
                RemoveFriend(userFriend);
            }
        }

        public void ClearDomainEvents()
        {
            DomainEvents.Clear();
        }
        #endregion
    }
}
