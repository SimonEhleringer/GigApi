using GigApi.Application.Interfaces;
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
    public class GigDbContext : IdentityDbContext<GigUser>, IGigDbContext
    {
        public GigDbContext(DbContextOptions<GigDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PlaylistSong>()
                .HasKey(p => new { p.PlaylistId, p.SongId });

            modelBuilder.Entity<PlaylistSong>()
                .HasOne(p => p.Playlist)
                .WithMany(p => p.PlaylistSongs)
                .HasForeignKey(p => p.PlaylistId);

            modelBuilder.Entity<PlaylistSong>()
                .HasOne(p => p.Song)
                .WithMany(p => p.PlaylistSongs)
                .HasForeignKey(p => p.SongId);

            modelBuilder.Entity<RefreshToken>()
                .HasOne(x => x.User)
                .WithMany(x => x.RefreshTokens)
                .HasForeignKey(x => x.UserId);
        }

        public DbSet<Playlist> Playlists { get; set; }

        public DbSet<Song> Songs { get; set; }

        public DbSet<PlaylistSong> PlaylistSongs { get; set; }

        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}
