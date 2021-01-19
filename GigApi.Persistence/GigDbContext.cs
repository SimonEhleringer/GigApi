using GigApi.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApi.Persistence
{
    public class GigDbContext : IdentityDbContext<GigUser>
    {
        public GigDbContext(DbContextOptions<GigDbContext> options) : base(options)
        {
        }
    }
}
