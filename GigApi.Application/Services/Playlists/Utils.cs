using GigApi.Application.Exceptions;
using GigApi.Application.Interfaces;
using GigApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApi.Application.Services.Playlists
{
    static class Utils
    {
        public static async Task<Playlist> GetByIdWithoutTrackingAsync(IGigDbContext context, Guid playlistId)
        {
            return await context.Playlists
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.PlaylistId == playlistId);
        }

        public static void FillSongs(this IList<PlaylistSong> playlistSongs, IGigDbContext context, Guid loggedInUserId)
        {
            // Retrieve Songs for join table
            playlistSongs.ToList()
                .ConvertAll(x =>
                    x.Song = context.Songs.FirstOrDefault(y => y.SongId == x.SongId));

            // Check if user owns all the songs
            foreach (var playlistSong in playlistSongs)
            {
                if (playlistSong.Song.UserId != loggedInUserId)
                {
                    throw new UserHasNoPermissionException();
                }
            }
        }
    }
}
