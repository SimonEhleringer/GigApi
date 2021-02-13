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
    public class PlaylistService
    {
        private readonly IGigDbContext _context;

        public PlaylistService(IGigDbContext context)
        {
            _context = context;
        }

        public async Task<IList<Playlist>> GetAllAsync(Guid loggedInUserId)
        {
            return await _context.Playlists
                .Where(x => x.UserId == loggedInUserId)
                .Include(x => x.PlaylistSongs)
                .ThenInclude(x => x.Song)
                .ToListAsync();
        }

        public async Task<Playlist> GetByIdAsync(Guid playlistId, Guid loggedInUserId)
        {
            var playlist = await _context.Playlists
                .Include(x => x.PlaylistSongs)
                .ThenInclude(x => x.Song)
                .SingleOrDefaultAsync(x => x.PlaylistId == playlistId);

            if (playlist.UserId != loggedInUserId)
            {
                throw new UserHasNoPermissionException();
            }

            return playlist;
        }

        public async Task<Playlist> CreateAsync(Playlist playlistToCreate, Guid loggedInUserId)
        {
            //// Retrieve Songs for join table
            //playlistToCreate.PlaylistSongs.ToList()
            //    .ConvertAll(x => 
            //    x.Song = _context.Songs.FirstOrDefault(y => y.SongId == x.SongId));

            //// Check if user owns all the songs
            //foreach (var playlistSong in playlistToCreate.PlaylistSongs)
            //{
            //    if (playlistSong.Song.UserId != loggedInUserId)
            //    {
            //        throw new UserHasNoPermissionException();
            //    }
            //}

            playlistToCreate.PlaylistSongs.FillSongs(_context, loggedInUserId);

            playlistToCreate.UserId = loggedInUserId;

            var createdPlaylist = await _context.Playlists.AddAsync(playlistToCreate);
            await _context.SaveChangesAsync();

            return createdPlaylist.Entity;
        }

        public async Task<Playlist> UpdateAsync(Playlist playlistToUpdate, Guid loggedInUserId)
        {
            var playlistToBeUpdated = await _context.Playlists
                .Include(x => x.PlaylistSongs)
                .ThenInclude(x => x.Song)
                .SingleOrDefaultAsync(x => x.PlaylistId == playlistToUpdate.PlaylistId);

            if (playlistToBeUpdated == null)
            {
                return null;
            }

            if (playlistToBeUpdated.UserId != loggedInUserId)
            {
                throw new UserHasNoPermissionException();
            }

            playlistToUpdate.PlaylistSongs.FillSongs(_context, loggedInUserId);

            playlistToBeUpdated.Name = playlistToUpdate.Name;
            playlistToBeUpdated.PlaylistSongs = playlistToUpdate.PlaylistSongs;

            var updatedPlaylist = _context.Playlists.Update(playlistToBeUpdated);
            await _context.SaveChangesAsync();

            return updatedPlaylist.Entity;
        }

        public async Task<bool> DeleteAsync(Guid playlistId, Guid loggedInUserId)
        {
            var playlistToDelete = await Utils.GetByIdWithoutTrackingAsync(_context, playlistId);

            if (playlistToDelete == null)
            {
                return false;
            }

            if (playlistToDelete.UserId != loggedInUserId)
            {
                throw new UserHasNoPermissionException();
            }

            _context.Playlists.Remove(playlistToDelete);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
