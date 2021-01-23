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
    public class SendFriendRequestCommand : IRequest<Response<bool>>
    {
        public Guid UserId { get; set; }
    }

    public class SendFriendRequestHandler : IRequestHandler<SendFriendRequestCommand, Response<bool>>
    {
        private readonly ISocialUserContext context;
        private readonly ICurrentUserService currentUser;
        private readonly ISocialUserRepository repository;

        public SendFriendRequestHandler(
            ISocialUserContext context, 
            ICurrentUserService currentUser, 
            ISocialUserRepository repository)
        {
            this.context = context;
            this.currentUser = currentUser;
            this.repository = repository;
        }
        public async Task<Response<bool>> Handle(SendFriendRequestCommand request, CancellationToken cancellationToken)
        {
            SocialUser socialUser = await repository.Find(currentUser.SocialId);
            SocialUser friend = await context.SocialUsers.FindAsync(request.UserId);
            if (friend == null)
            {
                return Response<bool>.Fail("Friend Request Failed", "User does not exist");
            }

            socialUser.SendFriendRequest(friend);

            context.SocialUsers.UpdateRange(socialUser);
            await context.SaveChangesAsync(cancellationToken);

            return Response<bool>.Success(true);
        }
    }
}
