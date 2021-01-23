using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using SocialApp.Application.Common.Interfaces;

namespace SocialApp.Api.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor httpContext;

        public CurrentUserService(IHttpContextAccessor httpContext)
        {
            this.httpContext = httpContext;
        }

        public Guid SocialId => Guid.Parse(httpContext.HttpContext.User?.FindFirst("SocialId")?.Value);

        public string IdentityId => httpContext.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}
