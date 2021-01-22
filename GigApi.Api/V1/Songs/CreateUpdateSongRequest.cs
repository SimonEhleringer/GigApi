using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GigApi.Api.V1.Songs
{
    public class CreateUpdateSongRequest
    {
        public string Title { get; set; }

        public string Interpreter { get; set; }

        public int Tempo { get; set; }
    }
}
