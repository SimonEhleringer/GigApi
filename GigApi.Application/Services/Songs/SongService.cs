using GigApi.Application.Exceptions;
using GigApi.Application.Interfaces;
using GigApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GigApi.Application.Services.Songs
{
    public class SongService
    {
        private readonly IGigDbContext _context;

        public SongService(IGigDbContext context)
        {
            _context = context;
        }

        public async Task<IList<Song>> GetAllAsync(Guid loggedInUserId)
        {
            return await _context.Songs
                .Where(x => x.UserId == loggedInUserId)
                .ToListAsync();
        }

        public async Task<Song> GetByIdAsync(Guid songId, Guid loggedInUserId)
        {
            var song = await _context.Songs.SingleOrDefaultAsync(s => s.SongId == songId);

            if (song.UserId != loggedInUserId)
            {
                throw new UserHasNoPermissionException();
            }

            return song;
        }

        public async Task<Song> CreateAsync(Song songToCreate, Guid loggedInUserId)
        {
            songToCreate.UserId = loggedInUserId;

            var createdSong = await _context.Songs.AddAsync(songToCreate);
            await _context.SaveChangesAsync();

            return createdSong.Entity;
        }

        public async Task<Song> UpdateAsync(Song songToUpdate, Guid loggedInUserId)
        {
            var songToBeUpdated = await Utils.GetByIdWithoutTrackingAsync(_context, songToUpdate.SongId);

            if (songToBeUpdated == null)
            {
                return null;
            }

            if (songToBeUpdated.UserId != loggedInUserId)
            {
                throw new UserHasNoPermissionException();
            }

            songToBeUpdated.Title = songToUpdate.Title;
            songToBeUpdated.Interpreter = songToUpdate.Interpreter;
            songToBeUpdated.Tempo = songToUpdate.Tempo;

            var updatedSong = _context.Songs.Update(songToBeUpdated);
            await _context.SaveChangesAsync();

            return updatedSong.Entity;
        }

        public async Task<bool> DeleteAsync(Guid songId, Guid loggedInUserId)
        {
            var songToDelete = await Utils.GetByIdWithoutTrackingAsync(_context, songId);

            if (songToDelete == null)
            {
                return false;
            }

            if (songToDelete.UserId != loggedInUserId)
            {
                throw new UserHasNoPermissionException();
            }

            _context.Songs.Remove(songToDelete);
            await _context.SaveChangesAsync();

            return true;
        } 
    }
}
