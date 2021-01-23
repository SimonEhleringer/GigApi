using GigApi.Application.Interfaces;
using GigApi.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GigApi.Application.Services.Authentication
{
    public static class Utils
    {
        public static async Task<AuthenticationResult> GetAuthenticationResultAsync(this GigUser user, AuthenticationResult preparedResult, IJwtSettings jwtSettings, IGigDbContext context)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("id", user.Id)
                }),
                Expires = DateTime.UtcNow.Add(jwtSettings.TokenLifeTime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var jwtToken = tokenHandler.CreateToken(tokenDescriptor);

            var refreshToken = new RefreshToken
            {
                JwtId = jwtToken.Id,
                UserId = user.Id,
                CreationTime = DateTime.UtcNow,
                ExpiryTime = DateTime.UtcNow.AddMonths(6)
            };

            await context.RefreshTokens.AddAsync(refreshToken);
            await context.SaveChangesAsync();

            preparedResult.Succeeded = true;
            preparedResult.JwtToken = tokenHandler.WriteToken(jwtToken);
            preparedResult.RefreshToken = refreshToken.Token.ToString();

            return preparedResult;
        }

        public static ClaimsPrincipal GetPrincipalFromJwtToken(string jwtToken, TokenValidationParameters tokenValidationParameters)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var principal = jwtTokenHandler.ValidateToken(jwtToken, tokenValidationParameters, out var validatedJwtToken);

                if (!IsJwtWithValidSecurityAlgorithm(validatedJwtToken))
                {
                    return null;
                }

                return principal;
            }
            catch
            {
                return null;
            }
        }

        public static bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedJwtToken)
        {
            return (validatedJwtToken is JwtSecurityToken jwtSecurityToken) &&
                jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
