using MediatR;
using SocialApp.Domain.Common.ValueObjects;
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
    public class CreateSocialUserCommand : IRequest<Response<Guid>>
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class CreateSocialUserCommandHandler : IRequestHandler<CreateSocialUserCommand, Response<Guid>>
    {
        private readonly IIdentityService service;
        private readonly ISocialUserRepository repository;
        private readonly ISocialUserContext context;

        public CreateSocialUserCommandHandler(
            IIdentityService service,
            ISocialUserRepository repository,
            ISocialUserContext context)
        {
            this.service = service;
            this.repository = repository;
            this.context = context;
        }
        public async Task<Response<Guid>> Handle(CreateSocialUserCommand request, CancellationToken cancellationToken)
        {
            var socialUserId = await service.FindByUsernameAsync(request.Username);
            if (!socialUserId.Succeeded)
            {
                var identityId = await service.CreateAsync(request.Username, request.Email, request.Password);
                if (identityId.Succeeded)
                {
                    SocialUser newUser = SocialUser
                        .Create(//Guid.Empty,
                                Guid.Parse(identityId.Data),
                                Identity.Create(identityId.Data),
                                Username.Create(request.Username),
                                EmailAddress.Create(request.Email),
                                Image.Default,
                                DateTime.UtcNow);

                    await context.SocialUsers.AddAsync(newUser, cancellationToken);
                    await context.SaveChangesAsync(cancellationToken);

                    return Response<Guid>.Success(newUser.Id);
                }
            }
            return Response<Guid>.Fail(Error.Create("Registration Error", ""));
        }
    }
}
