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
    public class BlockUserCommand : IRequest<Response<bool>>
    {
        public Guid SocialUserId { get; set; }
    }

    public class BlockUserCommandHandler : IRequestHandler<BlockUserCommand, Response<bool>>
    {
        private readonly ICurrentUserService currentUser;
        private readonly IApplicationDbContext context;
        private readonly ISocialUserRepository repository;

        public BlockUserCommandHandler(ICurrentUserService currentUser, IApplicationDbContext context, ISocialUserRepository repository)
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

            await repository.Save(blocker);
            await repository.Save(blocked);

            await context.SaveChangesAsync(cancellationToken);

            return Response<bool>.Success(true);
        }
    }
}
