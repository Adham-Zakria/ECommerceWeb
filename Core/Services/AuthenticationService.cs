using AutoMapper;
using Domain.Exceptions;
using Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic.FileIO;
using ServicesAbstraction;
using Shared.DataTransferObjects.Authentication;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AuthenticationService(UserManager<ApplicationUser> _userManager ,
        IOptions<JWTOptions> _jwtOptions , IMapper _mapper) : IAuthenticationService
    {
        public async Task<bool> CheckEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user != null;
        }

        public async Task<UserResponse> GetUserByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email)
                                         ?? throw new UserNotFoundException(email);

            return new UserResponse()
            {
                DisplayName = user.DisplayName,
                Email = user.Email!,
                Token = await GenerateToken(user)
            };
        }

        public async Task<AddressDto> GetUserAddressAsync(string email)
        {
            var user = await _userManager.Users
                                         .Include(u => u.Address)
                                         .FirstOrDefaultAsync(u=>u.Email==email) 
                                         ?? throw new UserNotFoundException(email);

            if (user.Address is not null) 
                return _mapper.Map<AddressDto>(user.Address);

            throw new AddressNotFoundException(user.UserName);
        }

        public async Task<AddressDto> UpdateUserAddressAsync(AddressDto addressDto, string email)
        {
            var user = await _userManager.Users
                                         .Include(u => u.Address)
                                         .FirstOrDefaultAsync(u => u.Email == email)
                                         ?? throw new UserNotFoundException(email);
            if (user.Address is not null)
            {  //update
                user.Address.FirstName = addressDto.FirstName;
                user.Address.LastName = addressDto.LastName;
                user.Address.Street = addressDto.Street;
                user.Address.City = addressDto.City;
                user.Address.Country = addressDto.Country;

            }
            else // create
            {
                user.Address = _mapper.Map<Address>(addressDto);
            }

            await _userManager.UpdateAsync(user);
            return _mapper.Map<AddressDto>(user.Address);
        }


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
                    Token = await GenerateToken(user) //3.generate token
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
                    Token = await GenerateToken(user) //generate token
                };
            }
            var errors=createdUser.Errors.Select(e=>e.Description).ToList();
            throw new BadRequestException(errors);
        }


        private async Task<string> GenerateToken(ApplicationUser user)
        {
            var jwtOpt = _jwtOptions.Value;
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Name, user.UserName!),
            };
            var roles= await _userManager.GetRolesAsync(user);

            foreach (var role in roles) 
                claims.Add(new Claim(ClaimTypes.Role, role));

            string secretKey = jwtOpt.SecretKey;   
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var credentials = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtOpt.Issuer,//
                audience: jwtOpt.Audience,//
                claims: claims,
                expires: DateTime.UtcNow.AddDays(jwtOpt.DurationInDays),
                signingCredentials: credentials
                );
            
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);

        }
    }
}
