using GigApi.Application.Interfaces;
using GigApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApi.Application.Services.Songs
{
    static class Utils
    {
        public static async Task<Song> GetByIdWithoutTrackingAsync(IGigDbContext context, Guid songId)
        {
            return await context.Songs.AsNoTracking().SingleOrDefaultAsync(s => s.SongId == songId);
        }
    }
}
