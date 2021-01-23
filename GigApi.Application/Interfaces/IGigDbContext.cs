using GigApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GigApi.Application.Interfaces
{
    public interface IGigDbContext
    {
        public DbSet<Playlist> Playlists { get; set; }

        public DbSet<Song> Songs { get; set; }

        public DbSet<PlaylistSong> PlaylistSongs { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
