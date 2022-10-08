using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using UsersNG.Data;
using UsersNG.DTO;
using UsersNG.Models;
using UsersNG.Shared;

namespace UsersNG.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly UsersNGContext _context;
        public AuthService(IConfiguration configuration, UsersNGContext context)
        {
            _configuration = configuration;
            _context = context;
        }
        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        public async Task<ServiceResponse<string>> Login(LoginDto request)
        {
            var user = await _context.User.Where(x => x.Email == request.Email).FirstOrDefaultAsync();
            var response = new ServiceResponse<string>();

            if (user == null || (user.Email != request.Email))
            {
                response.Success = false;
                response.Message = "User not found.";
                return response;
            }

            //if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            //{
            //    response.Success = false;
            //    response.Message = "Wrong password.";
            //    return response;
            //}

            if (user.Password != request.Password)
            {
                response.Success = false;
                response.Message = "Wrong password.";
                return response;
            }

            string token = CreateToken(user);
            response.Data = token;
            return response;
        }

        public async Task<ServiceResponse<User>> Register(UserDto request)
        {
            var response = new ServiceResponse<User>();
            //CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var check = await _context.User.Where(x => x.Email == request.Email).FirstOrDefaultAsync();

            if (check != null)
            {
                response.Success = false;
                response.Message = "EmailAlreadyExists";

                return response;
            }

            var user = new User();
            user.Email = request.Email;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Password = request.Password;
            //user.PasswordHash = passwordHash;
            //user.PasswordSalt = passwordSalt;

            _context.User.Add(user);
            await _context.SaveChangesAsync();

            response.Data = user;

            return response;
        }
    }
}
