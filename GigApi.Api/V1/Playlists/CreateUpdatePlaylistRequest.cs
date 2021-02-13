using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GigApi.Api.V1.Playlists
{
    public class CreateUpdatePlaylistRequest
    {
        public string Name { get; set; }

        public IList<Guid> SongIds { get; set; }
    }
}
