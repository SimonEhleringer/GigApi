using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GigApi.Api.V1.Playlists
{
    public class CreateUpdatePlaylistRequest
    {
        [Required(ErrorMessage = "Es wurde kein Name für die Playlist übermittelt.")]
        [MaxLength(50, ErrorMessage = "Der Name einer Playlist darf höchstens 50 Zeichen lang sein.")]
        public string Name { get; set; }

        public IList<Guid> SongIds { get; set; }
    }
}
