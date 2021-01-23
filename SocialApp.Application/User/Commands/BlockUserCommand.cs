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
    public class BlockUserCommand : IRequest<Response<bool>>
    {
        public Guid SocialUserId { get; set; }
    }

    public class BlockUserCommandHandler : IRequestHandler<BlockUserCommand, Response<bool>>
    {
        private readonly ICurrentUserService currentUser;
        private readonly ISocialUserContext context;
        private readonly ISocialUserRepository repository;

        public BlockUserCommandHandler(ICurrentUserService currentUser, ISocialUserContext context, ISocialUserRepository repository)
        {
            this.currentUser = currentUser;
            this.context = context;
            this.repository = repository;
        }
        public async Task<Response<bool>> Handle(BlockUserCommand request, CancellationToken cancellationToken)
        {
            SocialUser blocker = await repository.Find(currentUser.SocialId);

            SocialUser blocked = await repository.Find(request.SocialUserId);

            blocker.BlockUser(blocked);

            blocker.RemoveFriend(blocked);
            blocked.RemoveFriend(blocker);

            context.SocialUsers.UpdateRange(blocker, blocked);

            await context.SaveChangesAsync(cancellationToken);

            return Response<bool>.Success(true);
        }
    }
}
