using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GigApi.Api.V1.Authentication
{
    public class RegisterLoginResponse
    {
        public bool Succeeded { get; set; }

        public string JwtToken { get; set; }

        public IEnumerable<string> Errors { get; set; }
    }
}
