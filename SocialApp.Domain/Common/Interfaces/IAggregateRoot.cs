using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialApp.Domain.Common.Interfaces
{
    public interface IAggregateRoot : IDomainEntity, IHasDomainEvent
    {
        
    }
}
