using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GigApi.Api.V1
{
    public class ErrorResponse
    {
        public IList<string> Errors { get; set; } = new List<string>();
    }
}
