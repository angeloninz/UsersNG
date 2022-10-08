using UsersNG.DTO;
using UsersNG.Models;
using UsersNG.Shared;

namespace UsersNG.Services.AuthService
{
    public interface IAuthService
    {
        public Task<ServiceResponse<string>> Login(LoginDto request);
        public Task<ServiceResponse<User>> Register(UserDto request);
    }
}
