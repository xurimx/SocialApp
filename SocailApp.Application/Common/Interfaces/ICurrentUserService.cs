using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocailApp.Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        Guid SocialId { get; }
        string IdentityId { get; }
    }
}
