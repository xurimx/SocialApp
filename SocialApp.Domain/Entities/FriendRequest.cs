using SocialApp.Domain.Common;
using SocialApp.Domain.Common.Interfaces;
using SocialApp.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialApp.Domain.Entities
{
    public class FriendRequest : BaseEntity, IDomainEntity
    {
        public SocialUser Sender { get; set; }
        public SocialUser Receiver { get; set; }
        public RequestStatus Status { get; set; }

        private FriendRequest() { }        

        protected FriendRequest(SocialUser sender, SocialUser receiver, RequestStatus status)
        {
            Sender = sender;
            Receiver = receiver;
            Status = status;
        }

        private FriendRequest (Guid id, SocialUser sender, SocialUser receiver, RequestStatus status) 
            : this(sender, receiver, status)
        {
            Id = id;
        }


        public static FriendRequest Create(SocialUser sender, SocialUser friend)
        {
            return new FriendRequest(Guid.Empty, sender, friend, RequestStatus.Pending);
        }

        public void Accept()
        {
            Status = RequestStatus.Accepted;
        }

        public void Reject()
        {
            Status = RequestStatus.Rejected;
        }
    }
}
