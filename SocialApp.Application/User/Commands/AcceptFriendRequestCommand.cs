using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SocialApp.Application.Common.Interfaces;
using SocialApp.Application.Common.Interfaces.Repositories;
using SocialApp.Application.Common.Responses;

namespace SocialApp.Application.User.Commands
{
    public class AcceptFriendRequestCommand : IRequest<Response<bool>>
    {
        public Guid FriendRequest { get; set; }
    }

    public class AcceptFriendRequestCommandHandler : IRequestHandler<AcceptFriendRequestCommand, Response<bool>>
    {
        private readonly ISocialUserContext context;
        private readonly ICurrentUserService currentUser;
        private readonly ISocialUserRepository repository;

        public AcceptFriendRequestCommandHandler(
            ISocialUserContext context, 
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

            context.SocialUsers.Update(user);
            await context.SaveChangesAsync(cancellationToken);

            return Response<bool>.Success(true);
        }
    }
}
