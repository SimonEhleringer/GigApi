using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GigApi.Api.V1.Songs
{
    public class SongResponse
    {
        public Guid SongId { get; set; }

        public string Title { get; set; }

        public string Interpreter { get; set; }

        public int Tempo { get; set; }

        public string Notes { get; set; }
    }
}
