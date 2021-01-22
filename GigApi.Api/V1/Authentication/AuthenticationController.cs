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

        public AuthenticationController(AuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var authResponse = await _authenticationService.RegisterAsync(request.Username, request.Email, request.Password);

            var response = new RegisterLoginResponse
            {
                Succeeded = authResponse.Succeeded,
                JwtToken = authResponse.JwtToken,
                Errors = authResponse.Errors
            };

            if (!authResponse.Succeeded)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var authResponse = await _authenticationService.LoginAsync(request.Email, request.Password);

            var response = new RegisterLoginResponse
            {
                Succeeded = authResponse.Succeeded,
                JwtToken = authResponse.JwtToken,
                Errors = authResponse.Errors
            };

            if (!authResponse.Succeeded)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
    }
}
