using Microsoft.AspNetCore.Mvc;
using UsersNG.Models;
using UsersNG.Shared;

namespace UsersNG.Services.UserService
{
    public interface IUserService
    {
        public Task<ServiceResponse<List<User>>> GetUser();
        public Task<ServiceResponse<User>> GetUser(int id);
        public Task<ServiceResponse<User>> PutUser(int id, User user);
        public Task<ServiceResponse<User>> PostUser(User user);

        public Task<ServiceResponse<User>> DeleteUser(int id);
    }
}
