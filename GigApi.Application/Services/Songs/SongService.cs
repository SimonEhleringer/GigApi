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

        public async Task<IList<Song>> GetAllAsync()
        {
            return await _context.Songs.ToListAsync();
        }

        public async Task<Song> GetByIdAsync(Guid songId)
        {
            return await _context.Songs.SingleOrDefaultAsync(s => s.SongId == songId);
        }

        public async Task<Song> CreateAsync(Song songToCreate)
        {
            var createdSong = await _context.Songs.AddAsync(songToCreate);
            await _context.SaveChangesAsync();

            return createdSong.Entity;
        }

        public async Task<Song> UpdateAsync(Song songToUpdate)
        {
            var songToBeUpdated = await Utils.GetByIdWithoutTrackingAsync(_context, songToUpdate.SongId);

            if (songToBeUpdated == null)
            {
                return null;
            }

            var updatedSong = _context.Songs.Update(songToUpdate);
            await _context.SaveChangesAsync();

            return updatedSong.Entity;
        }

        public async Task<bool> DeleteAsync(Guid songId)
        {
            var songToDelete = await Utils.GetByIdWithoutTrackingAsync(_context, songId);

            if (songToDelete == null)
            {
                return false;
            }

            _context.Songs.Remove(songToDelete);
            await _context.SaveChangesAsync();

            return true;
        } 
    }
}
