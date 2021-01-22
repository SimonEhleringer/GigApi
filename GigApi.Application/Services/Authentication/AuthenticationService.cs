using GigApi.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using GigApi.Application.Interfaces;

namespace GigApi.Application.Services.Authentication
{
    public class AuthenticationService
    {
        private readonly UserManager<GigUser> _userManager;
        private readonly IJwtSettings _jwtSettings;

        public AuthenticationService(UserManager<GigUser> userManager, IJwtSettings jwtSettings)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;
        }

        public async Task<AuthenticationResult> RegisterAsync(string username, string email, string password)
        {
            var result = new AuthenticationResult
            {
                Succeeded = false,
                JwtToken = String.Empty,
                Errors = new List<string>(),
            };

            var existingUserByName = await _userManager.FindByNameAsync(username);

            if (existingUserByName != null)
            {
                result.Errors.Add("Dieser Benutzername ist bereits vergeben.");
            }

            var existingUserByEmail = await _userManager.FindByEmailAsync(email);

            if (existingUserByEmail != null)
            {
                result.Errors.Add("Diese E-Mail ist bereits vergeben.");
            }

            if (existingUserByName != null || existingUserByEmail != null)
            {
               return result;
            }

            var newUser = new GigUser
            {
                UserName = username,
                Email = email,
            };

            var createdUser = await _userManager.CreateAsync(newUser, password);

            if (!createdUser.Succeeded)
            {
                result.Errors = createdUser.Errors.Select(x => x.Description).ToList();
                return result;
            }

            //var tokenHandler = new JwtSecurityTokenHandler();
            //var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            //var tokenDescriptor = new SecurityTokenDescriptor
            //{
            //    Subject = new ClaimsIdentity(new[]
            //    {
            //        new Claim(JwtRegisteredClaimNames.Sub, newUser.UserName),
            //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            //        new Claim(JwtRegisteredClaimNames.Email, newUser.Email),
            //        new Claim("id", newUser.Id)
            //    }),
            //    Expires = DateTime.UtcNow.AddHours(1),
            //    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            //};

            //var jwtToken = tokenHandler.CreateToken(tokenDescriptor);

            
            //result.JwtToken = tokenHandler.WriteToken(jwtToken);

            result.JwtToken = newUser.GetJwtToken(_jwtSettings);
            result.Succeeded = true;

            return result;
        }

        public async Task<AuthenticationResult> LoginAsync(string email, string password)
        {
            var result = new AuthenticationResult
            {
                Succeeded = false,
                JwtToken = String.Empty,
                Errors = new List<string>()
            };

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                result.Errors.Add("Dieser Benutzer existiert nicht.");
                return result;
            }

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, password);

            if (!isPasswordCorrect)
            {
                result.Errors.Add("Die E-Mail oder das Passwort ist falsch.");
                return result;
            }

            result.JwtToken = user.GetJwtToken(_jwtSettings);
            result.Succeeded = true;

            return result;
        }
    }
}
