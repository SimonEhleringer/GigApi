using GigApi.Api.V1.Songs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GigApi.Api.V1.Playlists
{
    public class PlaylistResponse
    {
        public Guid PlaylistId { get; set; }

        public string Name { get; set; }

        public IEnumerable<SongResponse> Songs { get; set; }
}
}
