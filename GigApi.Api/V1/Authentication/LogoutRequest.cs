using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GigApi.Api.V1.Authentication
{
    public class LogoutRequest
    {
        public string RefreshToken { get; set; }
    }
}
