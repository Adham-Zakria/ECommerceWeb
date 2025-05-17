using Shared.DataTransferObjects.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesAbstraction
{
     public interface IAuthenticationService
     {
        Task<UserResponse> LoginAsync(LoginRequest loginRequest);
        Task<UserResponse> RegisterAsync(RegisterRequest registerRequest);
        Task<bool> CheckEmailAsync(string email);
        Task<UserResponse> GetUserByEmailAsync(string email);
        Task<AddressDto> GetUserAddressAsync(string email);
        Task<AddressDto> UpdateUserAddressAsync(AddressDto addressDto,string email);
     } 
}