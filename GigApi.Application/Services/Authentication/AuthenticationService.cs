using GigApi.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using GigApi.Application.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;

namespace GigApi.Application.Services.Authentication
{
    public class AuthenticationService
    {
        private readonly UserManager<GigUser> _userManager;
        private readonly IJwtSettings _jwtSettings;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly IGigDbContext _context;

        public AuthenticationService(UserManager<GigUser> userManager, IJwtSettings jwtSettings, TokenValidationParameters tokenValidationParameters, IGigDbContext context)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;
            _tokenValidationParameters = tokenValidationParameters;
            _context = context;
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

            return await newUser.GetAuthenticationResultAsync(result, _jwtSettings, _context);
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

            return await user.GetAuthenticationResultAsync(result, _jwtSettings, _context);
        }

        public async Task<AuthenticationResult> RefreshJwtTokenAsync(string jwtToken, string refreshToken)
        {
            var result = new AuthenticationResult
            {
                Succeeded = false,
                JwtToken = String.Empty,
                Errors = new List<string>()
            };

            var validatedJwtToken = Utils.GetPrincipalFromJwtToken(jwtToken, _tokenValidationParameters);

            if (validatedJwtToken == null)
            {
                result.Errors.Add("Jwt-Token ist nicht valide.");
                return result;
            }

            var expiryTimeUnix =
                long.Parse(validatedJwtToken.Claims.Single(x =>
                    x.Type == JwtRegisteredClaimNames.Exp).Value);

            var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(expiryTimeUnix);

            if (expiryDateTimeUtc > DateTime.UtcNow)
            {
                result.Errors.Add("Jwt-Token ist noch nicht abgelaufen.");
                return result;
            }

            var jti = validatedJwtToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            var storedRefreshToken = await _context.RefreshTokens.SingleOrDefaultAsync(x => x.Token.ToString() == refreshToken);

            if (storedRefreshToken == null)
            {
                result.Errors.Add("Dieses Refresh-Token existiert nicht.");
                return result;
            }

            if (DateTime.UtcNow > storedRefreshToken.ExpiryTime)
            {
                result.Errors.Add("Dieses Refresh-Token ist abgelaufen.");
                return result;
            }

            if (storedRefreshToken.Invalidated)
            {
                result.Errors.Add("Dieses Refresh-Token wurde deaktiviert.");
                return result;
            }

            if (storedRefreshToken.Used)
            {
                result.Errors.Add("Dieses Refresh-Token wurde benutzt.");
                return result;
            }

            if (storedRefreshToken.JwtId != jti)
            {
                result.Errors.Add("Dieses Refresh-Token passt nicht zum Jwt-Token");
                return result;
            }

            storedRefreshToken.Used = true;
            _context.RefreshTokens.Update(storedRefreshToken);
            await _context.SaveChangesAsync();

            var user = await _userManager.FindByIdAsync(validatedJwtToken.Claims.Single(x => x.Type == "id").Value);

            return await user.GetAuthenticationResultAsync(result, _jwtSettings, _context);
        }
    }
}
