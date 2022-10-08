using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UsersNG.Models;

namespace UsersNG.Data
{
    public class UsersNGContext : DbContext
    {
        public UsersNGContext (DbContextOptions<UsersNGContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id=1,
                    Email="admin@gmail.com",
                    FirstName="Admin",
                    LastName="Admin",
                    Password="admin123"
                }
            );
        }
        public DbSet<UsersNG.Models.User> User { get; set; } = default!;
    }
}
