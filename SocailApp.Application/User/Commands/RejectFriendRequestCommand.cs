﻿using MediatR;
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
    public class RejectFriendRequestCommand : IRequest<Response<bool>>
    {
        public Guid FriendRequestId { get; set; }
    }

    public class RejectFriendRequestCommandHandler : IRequestHandler<RejectFriendRequestCommand, Response<bool>>
    {
        private readonly IApplicationDbContext context;
        private readonly ICurrentUserService currentUser;
        private readonly ISocialUserRepository repository;

        public RejectFriendRequestCommandHandler(
            IApplicationDbContext context, 
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

            await repository.Save(socialUser);
            await context.SaveChangesAsync(cancellationToken);

            return Response<bool>.Success(true);
        }
    }
}
