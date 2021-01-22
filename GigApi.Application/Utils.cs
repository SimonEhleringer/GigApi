using GigApi.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GigApi.Application
{
    public static class Utils
    {
        public static async Task<int> SaveChangesAsync(this IGigDbContext context)
        {
            return await context.SaveChangesAsync(CancellationToken.None);
        }
    }
}
