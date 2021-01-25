using AutoMapper;
using GigApi.Application.Services.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GigApi.Api.V1.Authentication
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly AuthenticationService _authenticationService;
        private readonly IMapper _mapper;

        public AuthenticationController(AuthenticationService authenticationService, IMapper mapper)
        {
            _authenticationService = authenticationService;
            _mapper = mapper;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var authResponse = await _authenticationService.RegisterAsync(request.Username, request.Email, request.Password);

            return ReturnResponseByAuthenticationResult(authResponse);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var authResponse = await _authenticationService.LoginAsync(request.Email, request.Password);

            return ReturnResponseByAuthenticationResult(authResponse);
        }

        [HttpPost("Refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshLogoutRequest request)
        {
            var authResponse = await _authenticationService.RefreshJwtTokenAsync(request.JwtToken, request.RefreshToken);

            return ReturnResponseByAuthenticationResult(authResponse);
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout([FromBody] RefreshLogoutRequest request)
        {
            var authResponse = await _authenticationService.LogoutAsync(request.JwtToken, request.RefreshToken);
            
            if (!authResponse.Succeeded)
            {
                return BadRequest(_mapper.Map<ErrorResponse>(authResponse));
            }

            return NoContent();
        }

        private IActionResult ReturnResponseByAuthenticationResult(AuthenticationResult authenticationResult)
        {
            if (!authenticationResult.Succeeded)
            {
                return BadRequest(_mapper.Map<ErrorResponse>(authenticationResult));
            }

            return Ok(_mapper.Map<AuthenticationResponse>(authenticationResult));
        }
    }
}
