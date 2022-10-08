using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersNG.Models;

namespace UsersNG.Tests.Fixtures
{
    public static class UsersFixture
    {
        public static List<User> GetTestUsers() => new()
        {
            new User
            {
                Id=1,
                FirstName="Peter",
                LastName="Parker",
                Email="peter.parker@gmail.com",
                Password="Asdf1234"
            },
            new User
            {
                Id=2,
                FirstName="Tony",
                LastName="Stark",
                Email="tony.stark@gmail.com",
                Password="Qwerty123"
            }
        };

    }
}
