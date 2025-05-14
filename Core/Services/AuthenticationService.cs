using Domain.Exceptions;
using Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
using ServicesAbstraction;
using Shared.DataTransferObjects.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AuthenticationService(UserManager<ApplicationUser> _userManager) : IAuthenticationService
    {
        public async Task<UserResponse> LoginAsync(LoginRequest loginRequest)
        {
            //1.find the user with this email
            var user = await _userManager.FindByEmailAsync(loginRequest.Email) ??
                throw new UserNotFoundException(loginRequest.Email);

            //2.check the password for this user
            var isValidPass = await _userManager.CheckPasswordAsync(user, loginRequest.Password);
            if (isValidPass) 
            {
                return new UserResponse()
                {
                    Email = user.Email,
                    DisplayName = user.DisplayName,
                    Token = "JWTToken" //3.generate token
                };
            }

            throw new UnAuthorizedException();
        }

        public async Task<UserResponse> RegisterAsync(RegisterRequest registerRequest)
        {
            var user = new ApplicationUser()
            {
                Email = registerRequest.Email,
                UserName = registerRequest.DisplayName,
                DisplayName = registerRequest.DisplayName,
                PhoneNumber = registerRequest.PhoneNumber,
            };
            var createdUser = await _userManager.CreateAsync(user, registerRequest.Password);
            if (createdUser.Succeeded)
            {
                return new UserResponse()
                {
                    Email=user.Email,
                    DisplayName = user.DisplayName,
                    Token = "JWTToken" //generate token
                };
            }
            var errors=createdUser.Errors.Select(e=>e.Description).ToList();
            throw new BadRequestException(errors);
        }
    }
}
