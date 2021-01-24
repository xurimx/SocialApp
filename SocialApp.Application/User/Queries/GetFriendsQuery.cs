using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialApp.Application.Common.Interfaces;
using SocialApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocialApp.Application.User.Queries
{
    public class GetFriendsQuery : IRequest<IEnumerable<SocialUser>>
    {
    }

    public class GetFriendsQueryHandler : IRequestHandler<GetFriendsQuery, IEnumerable<SocialUser>>
    {
        private readonly ISocialUserContext context;
        private readonly ICurrentUserService currentUserService;

        public GetFriendsQueryHandler(ISocialUserContext context, ICurrentUserService currentUserService)
        {
            this.context = context;
            this.currentUserService = currentUserService;
        }
        public async Task<IEnumerable<SocialUser>> Handle(GetFriendsQuery request, CancellationToken cancellationToken)
        {
            List<SocialUser> friends = await context.SocialUsers
                .Include(x => x.Friends)
                .ThenInclude(x => x.Friend)
                .Where(x => x.Id == currentUserService.SocialId)
                .SelectMany(x => x.Friends.Select(f => f.Friend))
                .ToListAsync();

            return friends;
        }
    }
}
