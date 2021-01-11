using MediatR;
using Microsoft.EntityFrameworkCore;
using SocailApp.Application.Common.Interfaces;
using SocailApp.Application.Common.Interfaces.Repositories;
using SocailApp.Application.Common.Responses;
using SocialApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocailApp.Application.User.Commands
{
    public class AcceptFriendRequestCommand : IRequest<Response<bool>>
    {
        public Guid FriendRequest { get; set; }
    }

    public class AcceptFriendRequestCommandHandler : IRequestHandler<AcceptFriendRequestCommand, Response<bool>>
    {
        private readonly IApplicationDbContext context;
        private readonly ICurrentUserService currentUser;
        private readonly ISocialUserRepository repository;

        public AcceptFriendRequestCommandHandler(
            IApplicationDbContext context, 
            ICurrentUserService currentUser, 
            ISocialUserRepository repository)
        {
            this.context = context;
            this.currentUser = currentUser;
            this.repository = repository;
        }
        public async Task<Response<bool>> Handle(AcceptFriendRequestCommand request, CancellationToken cancellationToken)
        {
            SocialUser user = await repository.Find(currentUser.SocialId);

            var friendRequest = await context.FriendRequests.FindAsync(request.FriendRequest);

            //user.AcceptFriendRequest(friendRequest);

            await repository.Save(user);
            await context.SaveChangesAsync(cancellationToken);

            return Response<bool>.Success(true);
        }
    }
}
