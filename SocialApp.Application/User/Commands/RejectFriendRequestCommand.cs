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
    public class RejectFriendRequestCommand : IRequest<Response<bool>>
    {
        public Guid FriendRequestId { get; set; }
    }

    public class RejectFriendRequestCommandHandler : IRequestHandler<RejectFriendRequestCommand, Response<bool>>
    {
        private readonly ISocialUserContext context;
        private readonly ICurrentUserService currentUser;
        private readonly ISocialUserRepository repository;

        public RejectFriendRequestCommandHandler(
            ISocialUserContext context, 
            ICurrentUserService currentUser, 
            ISocialUserRepository repository)
        {
            this.context = context;
            this.currentUser = currentUser;
            this.repository = repository;
        }
        public async Task<Response<bool>> Handle(RejectFriendRequestCommand request, CancellationToken cancellationToken)
        {
            SocialUser socialUser = await repository.Find(currentUser.SocialId);

            FriendRequest friendRequest = socialUser.PendingFriendRequests.Where(x => x.Id == request.FriendRequestId).FirstOrDefault();

            socialUser.RejectFriendRequest(friendRequest);

            context.SocialUsers.Update(socialUser);
            await context.SaveChangesAsync(cancellationToken);

            return Response<bool>.Success(true);
        }
    }
}
