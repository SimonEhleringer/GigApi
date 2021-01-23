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
        [EmailAddress(ErrorMessage = "Die E-Mail Adresse ist ungültig.")]
        [MaxLength(256, ErrorMessage = "Die E-Mail Adresse darf höchstens 256 Zeichen lang sein.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Es wurde kein Passwort übermittelt.")]
        [MaxLength(256, ErrorMessage = "Das Passwort darf höchstens 256 Zeichen lang sein.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{6,}$", 
            ErrorMessage = "Das Password muss mindestens sechs Zeichen lang sein, einen Groß- und Kleinbuchstaben sowie eine Zahl und ein Sonderzeichen enthalten.")]
        public string Password { get; set; }
    }
}
