using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApi.Domain.Entities
{
    public class Playlist
    {
        public Guid PlaylistId { get; set; }

        public string Name { get; set; }

        public IList<PlaylistSong> PlaylistSongs { get; set; }
    }
}
