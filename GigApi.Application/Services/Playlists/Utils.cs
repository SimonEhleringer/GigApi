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
        public static void FillSongs(this IList<PlaylistSong> playlistSongs, IGigDbContext context, Guid loggedInUserId)
        {
            // Retrieve Songs for join table
            playlistSongs.ToList()
                .ConvertAll(x =>
                    x.Song = context.Songs.FirstOrDefault(y => y.SongId == x.SongId));

            // Check if user owns all the songs
            foreach (var playlistSong in playlistSongs)
            {
                if (playlistSong.Song == null)
                {
                    throw new SongDoesNotExistException(playlistSong.SongId);
                }

                if (playlistSong.Song.UserId != loggedInUserId)
                {
                    throw new UserHasNoPermissionException();
                }
            }

            // Find entries with same PlaylistId SongId combination
            var duplicates = playlistSongs
                .GroupBy(x => x.SongId)
                .SelectMany(x => x.Skip(1))
                .ToList();

            if (duplicates.Count > 0)
            {
                var firstDuplicate = duplicates[0];

                throw new SongMultipleTimesInPlaylistException(firstDuplicate.SongId);
            }
        }

        public static void OrderPlaylistSongs(this Playlist playlist)
        {
            playlist.PlaylistSongs = playlist.PlaylistSongs
                    .OrderBy(x => x.IndexInPlaylist)
                    .ToList();
        }
    }
}
