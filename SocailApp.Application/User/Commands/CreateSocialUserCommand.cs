using MediatR;
using SocailApp.Application.Common.Interfaces;
using SocailApp.Application.Common.Interfaces.Repositories;
using SocailApp.Application.Common.Responses;
using SocialApp.Domain.Common.ValueObjects;
using SocialApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocailApp.Application.User.Commands
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
        private readonly IApplicationDbContext context;

        public CreateSocialUserCommandHandler(
            IIdentityService service, 
            ISocialUserRepository repository, 
            IApplicationDbContext context)
        {
            this.service = service;
            this.repository = repository;
            this.context = context;
        }
        public async Task<Response<Guid>> Handle(CreateSocialUserCommand request, CancellationToken cancellationToken)
        {
            string socialUserId = await service.FindByUsernameAsync(request.Username);
            if (socialUserId == null)
            {
                string identityId = await service.CreateAsync(request.Username, request.Email, request.Password);
                SocialUser newUser = SocialUser
                    .Create(Guid.Empty,
                            Identity.Create(identityId),
                            Username.Create(request.Username),
                            EmailAddress.Create(request.Email),
                            Image.Default,
                            DateTime.UtcNow);

                await repository.Save(newUser);
                await context.SaveChangesAsync(cancellationToken);

                return Response<Guid>.Success(newUser.Id);
            }
            return Response<Guid>.Fail(Error.Create("Registration Error", ""));
        }
    }
}
