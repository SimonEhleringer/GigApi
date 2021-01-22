using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GigApi.Api.V1.Playlists
{
    public class CreatePlaylistRequest
    {
        public string Name { get; set; }

        public IList<int> SongIds { get; set; }
    }
}
