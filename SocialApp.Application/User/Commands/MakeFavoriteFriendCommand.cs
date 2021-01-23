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
    public class MakeFavoriteFriendCommand : IRequest<Response<bool>>
    {
        public Guid FriendId { get; set; }
    }

    public class MakefavoriteFriendCommand : IRequestHandler<MakeFavoriteFriendCommand, Response<bool>>
    {
        private readonly ISocialUserContext context;
        private readonly ICurrentUserService currentUser;
        private readonly ISocialUserRepository repository;

        public MakefavoriteFriendCommand(
            ISocialUserContext context, 
            ICurrentUserService currentUser, 
            ISocialUserRepository repository)
        {
            this.context = context;
            this.currentUser = currentUser;
            this.repository = repository;
        }

        public async Task<Response<bool>> Handle(MakeFavoriteFriendCommand request, CancellationToken cancellationToken)
        {
            SocialUser socialUser = await repository.Find(currentUser.SocialId);

            Error error = Error.Create("Friend Request Error", "");
            if (socialUser != null)
            {
                UserFriend friend = socialUser.Friends.Where(x => x.Friend.Id == request.FriendId).FirstOrDefault();
                if (friend != null)
                {
                    friend.MakeFavorite();

                    context.SocialUsers.Update(socialUser);
                    await context.SaveChangesAsync(cancellationToken);
                    
                    return Response<bool>.Success(true);
                }
                error.AddError("friend", "friend was not found");
            }
            error.AddError("user", "current user was not found");

            return Response<bool>.Fail(error);
        }
    }
}
