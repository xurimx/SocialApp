using SocialApp.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialApp.Domain.Enumerations
{
    public class RequestStatus : Enumeration
    {
        public static RequestStatus Pending = new RequestStatus(1, nameof(Pending));
        public static RequestStatus Accepted = new RequestStatus(2, nameof(Accepted));
        public static RequestStatus Rejected = new RequestStatus(3, nameof(Rejected));

        public RequestStatus(int id, string name) : base(id, name) { }
    }
}
