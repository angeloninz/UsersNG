using System.ComponentModel.DataAnnotations.Schema;

namespace UsersNG.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        //public byte[] PasswordHash { get; set; } = new byte[32];    
        //public byte[] PasswordSalt { get; set; } = new byte[32];
    }
}
