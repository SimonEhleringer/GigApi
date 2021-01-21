using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApi.Domain.Entities
{
    public class Song
    {
        public Guid SongId { get; set; }

        public string Title { get; set; }

        public string Interpreter { get; set; }

        public int Tempo { get; set; }

        public IList<Playlist> Playlists { get; set; }
    }
}
