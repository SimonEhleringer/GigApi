using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GigApi.Api.V1.Authentication
{
    public class RegisterRequest
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}
