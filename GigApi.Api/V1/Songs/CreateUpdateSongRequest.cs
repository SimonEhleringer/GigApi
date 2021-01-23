using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GigApi.Api.V1.Songs
{
    public class CreateUpdateSongRequest
    {
        [Required]
        [MinLength(1)]
        [MaxLength(50)]
        public string Title { get; set; }

        [MaxLength(50)]
        public string Interpreter { get; set; }

        [Required]
        [Range(1, 999)]
        public int Tempo { get; set; }
    }
}
