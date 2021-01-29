using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace GigApi.Api.V1.Authentication
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Es wurde keine E-Mail Adresse übermittelt.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Es wurde kein Passwort übermittelt.")]
        public string Password { get; set; }
    }
}
