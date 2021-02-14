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
            var playlists = await _context.Playlists
                .Where(x => x.UserId == loggedInUserId)
                .Include(x => x.PlaylistSongs)
                .ThenInclude(x => x.Song)
                .OrderBy(x => x.Name)
                .ToListAsync();

            //for (int i = 0; i < playlists.Count; i++)
            //{
            //    playlists[i].PlaylistSongs = playlists[i].PlaylistSongs
            //        .OrderByDescending(x => x.IndexInPlaylist)
            //        .ToList();
            //}

            foreach (var playlist in playlists)
            {
                playlist.OrderPlaylistSongs();
            }

            return playlists;
        }

        public async Task<Playlist> GetByIdAsync(Guid playlistId, Guid loggedInUserId)
        {
            var playlist = await _context.Playlists
                .Include(x => x.PlaylistSongs)
                .ThenInclude(x => x.Song)
                .SingleOrDefaultAsync(x => x.PlaylistId == playlistId);

            if (playlist == null)
            {
                return null;
            }

            if (playlist.UserId != loggedInUserId)
            {
                throw new UserHasNoPermissionException();
            }

            playlist.OrderPlaylistSongs();

            return playlist;
        }

        public async Task<Playlist> CreateAsync(Playlist playlistToCreate, Guid loggedInUserId)
        {
            playlistToCreate.PlaylistSongs.FillSongs(_context, loggedInUserId);

            playlistToCreate.UserId = loggedInUserId;

            var createdPlaylist = await _context.Playlists.AddAsync(playlistToCreate);
            await _context.SaveChangesAsync();

            return createdPlaylist.Entity;
        }

        public async Task<Playlist> UpdateAsync(Playlist playlistToUpdate, Guid loggedInUserId)
        {
            var playlistToBeUpdated = await GetByIdAsync(playlistToUpdate.PlaylistId, loggedInUserId);

            playlistToUpdate.PlaylistSongs.FillSongs(_context, loggedInUserId);

            playlistToBeUpdated.Name = playlistToUpdate.Name;
            playlistToBeUpdated.PlaylistSongs = playlistToUpdate.PlaylistSongs;

            var updatedPlaylist = _context.Playlists.Update(playlistToBeUpdated);
            await _context.SaveChangesAsync();

            return updatedPlaylist.Entity;
        }

        public async Task<bool> DeleteAsync(Guid playlistId, Guid loggedInUserId)
        {
            var playlistToDelete = await _context.Playlists
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.PlaylistId == playlistId);

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
