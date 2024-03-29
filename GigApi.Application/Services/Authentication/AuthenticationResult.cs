﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigApi.Application.Services.Authentication
{
    public class AuthenticationResult
    {
        public string JwtToken { get; set; }

        public string RefreshToken { get; set; }

        public bool Succeeded { get; set; }

        public IList<string> Errors { get; set; }
    }
}
