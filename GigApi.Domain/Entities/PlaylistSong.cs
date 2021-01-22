using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApi.Domain.Entities
{
    public class PlaylistSong
    {
        public Guid PlaylistId { get; set; }

        public Playlist Playlist { get; set; }

        public Guid SongId { get; set; }

        public Song Song { get; set; }

        public int IndexInPlaylist { get; set; }
    }
}
