using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GigApi.Api.V1.Authentication
{
    public class RefreshLogoutRequest
    {
        [Required(ErrorMessage = "Es wurde kein Jwt-Token übermittelt.")]
        public string JwtToken { get; set; }

        [Required(ErrorMessage = "Es wurde kein Refresh-Token übermittelt.")]
        public string RefreshToken { get; set; }
    }
}
