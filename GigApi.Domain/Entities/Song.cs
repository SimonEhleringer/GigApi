using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApi.Domain.Entities
{
    public class Song
    {
        public Guid SongId { get; set; }

        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        [MaxLength(50)]
        public string Interpreter { get; set; }

        [Required]
        [Range(1, 999)]
        public int Tempo { get; set; }

        public IList<PlaylistSong> PlaylistSongs { get; set; }

        [Required]
        public Guid UserId { get; set; }
    }
}
