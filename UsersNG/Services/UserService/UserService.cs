using Microsoft.EntityFrameworkCore;
using UsersNG.Data;
using UsersNG.Models;
using UsersNG.Shared;

namespace UsersNG.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly UsersNGContext _context;
        public UserService(UsersNGContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<User>> DeleteUser(int id)
        {
            var user = await _context.User.FindAsync(id);
            var response = new ServiceResponse<User>();

            response.Data = user;

            if (user == null)
            {
                response.Success = false;
                response.Message = "NotFound";

                return response;
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            response.Message = "NoContent";

            return response;
        }

        public async Task<ServiceResponse<List<User>>> GetUser()
        {
            var response = new ServiceResponse<List<User>>();
            response.Data = await _context.User.ToListAsync();
            return response;
        }

        public async Task<ServiceResponse<User>> GetUser(int id)
        {
            var user = await _context.User.FindAsync(id);
            var response = new ServiceResponse<User>();

            response.Data = user;

            if (user == null)
            {
                response.Success = false;
                response.Message = "NotFound";

                return response;
            }

            response.Message = "NoContent";

            return response;
        }

        public async Task<ServiceResponse<User>> PostUser(User user)
        {
            var response = new ServiceResponse<User>();
            var check = await _context.User.Where(x => x.Email == user.Email).FirstOrDefaultAsync();

            if (check != null)
            {
                response.Success = false;
                response.Message = "EmailAlreadyExists";

                return response;
            }

            _context.User.Add(user);
            await _context.SaveChangesAsync();

            response.Data = user;

            return response;
        }

        public async Task<ServiceResponse<User>> PutUser(int id, User user)
        {
            var response = new ServiceResponse<User>();
            if (id != user.Id)
            {
                response.Success = false;
                response.Message = "BadRequest";

                return response;
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    response.Success = false;
                    response.Message = "NotFound";

                    return response;
                }
                else
                {
                    throw;
                }
            }

            response.Message = "NoContent";

            return response;
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}
