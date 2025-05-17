using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicesAbstraction;
using Shared.DataTransferObjects.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController(IServiceManager _serviceManager) : ControllerBase
    {
        [HttpPost("login")]
        public async Task<ActionResult<UserResponse>> Login(LoginRequest loginRequest)
        {
            return Ok( await _serviceManager.AuthenticationService.LoginAsync(loginRequest));
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserResponse>> Register(RegisterRequest registerRequest)
        {
            return Ok(await _serviceManager.AuthenticationService.RegisterAsync(registerRequest));
        }

        [Authorize]
        [HttpGet("CheckEmail")]
        public async Task<ActionResult<bool>> CheckEmail(string email)
        {
            return Ok(await _serviceManager.AuthenticationService.CheckEmailAsync(email));
        }

        [Authorize]
        [HttpGet("GetUser")]
        public async Task<ActionResult<UserResponse>> GetUserByEmail()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);  // Get the email from the token
            return Ok(await _serviceManager.AuthenticationService.GetUserByEmailAsync(email));
        }

        [Authorize]
        [HttpGet("GetAddress")]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            return Ok(await _serviceManager.AuthenticationService.GetUserAddressAsync(email));
        }

        [Authorize]
        [HttpPut("UpdateAddress")]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto address)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            return Ok(await _serviceManager.AuthenticationService.UpdateUserAddressAsync(address,email));
        }

    }
}
