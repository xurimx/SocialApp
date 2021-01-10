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
    public class SendFriendRequestCommand : IRequest<Response<bool>>
    {
        public Guid UserId { get; set; }
    }

    public class SendFriendRequestHandler : IRequestHandler<SendFriendRequestCommand, Response<bool>>
    {
        private readonly IApplicationDbContext context;
        private readonly ICurrentUserService currentUser;
        private readonly ISocialUserRepository repository;

        public SendFriendRequestHandler(
            IApplicationDbContext context, 
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
            socialUser.SendFriendRequest(friend);

            await repository.Save(socialUser);
            await context.SaveChangesAsync(cancellationToken);

            return Response<bool>.Success(true);
        }
    }
}
