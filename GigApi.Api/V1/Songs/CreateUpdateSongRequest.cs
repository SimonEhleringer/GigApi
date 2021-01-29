using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GigApi.Api.V1.Songs
{
    public class CreateUpdateSongRequest
    {
        [Required(ErrorMessage = "Es wurde kein Song Titel übermittelt.")]
        [MaxLength(50, ErrorMessage = "Der Song Titel darf höchstens 50 Zeichen lang sein.")]
        public string Title { get; set; }

        [MaxLength(50, ErrorMessage = "Der Name des Interpreters darf höchstens 50 Zeichen lang sein.")]
        public string Interpreter { get; set; }

        [Required(ErrorMessage = "Es wurde kein Song Temp übermittelt.")]
        [Range(1, 999, ErrorMessage = "Das Tempo darf nur zwischen 1 und 999 BPM sein.")]
        public int Tempo { get; set; }

        [MaxLength(256, ErrorMessage = "Die Notizen dürfen höchstens 256 Zeichen lang sein.")]
        public string Notes { get; set; }
    }
}
