using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SocialApp.Identity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialApp.Identity.Data
{
    public class SocialIdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public SocialIdentityDbContext(DbContextOptions<SocialIdentityDbContext> options) : base(options)
        {

        }   
    }
}
