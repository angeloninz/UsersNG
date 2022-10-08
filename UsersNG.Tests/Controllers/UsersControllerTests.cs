using Microsoft.AspNetCore.Mvc;
using Moq;
using UsersNG.Controllers;
using UsersNG.Models;
using UsersNG.Services.UserService;
using UsersNG.Shared;

namespace UsersNG.Tests.Controllers
{
    public class UsersControllerTests
    {
        private readonly Mock<IUserService> _userService;
        public UsersControllerTests()
        {
            _userService = new Mock<IUserService>();
        }

        [Fact]
        public async void Get_Users_ReturnsOk()
        {
            //Arrange
            var users = Fixtures.UsersFixture.GetTestUsers();
            var response = new ServiceResponse<List<User>>
            {
                Data = users
            };
            _userService.Setup(x => x.GetUser()).ReturnsAsync(response);
            var controller = new UsersController(_userService.Object);

            //Act
            var result = await controller.GetUsers();
            var obj = result as ObjectResult;
            var data = obj?.Value as ServiceResponse<List<User>>;

            //Assert
            Assert.Equal(200, obj?.StatusCode);
            Assert.Equal(users.Count(), data?.Data?.Count());
        }

        [Fact]
        public async void Get_User_ReturnsOk()
        {
            //Arrange
            var users = Fixtures.UsersFixture.GetTestUsers();
            var response = new ServiceResponse<User>
            {
                Data = users[0]
            };
            _userService.Setup(x => x.GetUser(1)).ReturnsAsync(response);
            var controller = new UsersController(_userService.Object);

            //Act
            var result = await controller.GetUser(1);
            var obj = result as ObjectResult;
            var data = obj?.Value as ServiceResponse<User>;

            //Assert
            Assert.Equal(200, obj?.StatusCode);
            Assert.Equal(response.Data.Id, data?.Data?.Id);
        }

        [Fact]
        public async void Get_User_ReturnsNotFound()
        {
            //Arrange
            var users = Fixtures.UsersFixture.GetTestUsers();
            var response = new ServiceResponse<User>
            {
                Data = null,
                Success = false,
                Message = "NotFound"
            };
            _userService.Setup(x => x.GetUser(1)).ReturnsAsync(response);
            var controller = new UsersController(_userService.Object);

            //Act
            var result = await controller.GetUser(1);
            var obj = result as NotFoundResult;

            //Assert
            Assert.Equal(404, obj?.StatusCode);
        }

        [Fact]
        public async void Post_User_ReturnsCreated()
        {
            //Arrange
            var users = Fixtures.UsersFixture.GetTestUsers();
            var response = new ServiceResponse<User>
            {
                Data = users[0]
            };
            _userService.Setup(x => x.PostUser(users[0])).ReturnsAsync(response);
            var controller = new UsersController(_userService.Object);

            //Act
            var result = await controller.PostUser(users[0]);
            var obj = result as ObjectResult;
            var data = obj?.Value as User;

            //Assert
            Assert.Equal(201, obj?.StatusCode);
            Assert.Equal(response.Data.Id, data?.Id);
        }

        [Fact]
        public async void Post_User_ReturnsConflict()
        {
            //Arrange
            var users = Fixtures.UsersFixture.GetTestUsers();
            var response = new ServiceResponse<User>
            {
                Success = false
            };
            _userService.Setup(x => x.PostUser(users[0])).ReturnsAsync(response);
            var controller = new UsersController(_userService.Object);

            //Act
            var result = await controller.PostUser(users[0]);
            var obj = result as ConflictResult;

            //Assert
            Assert.Equal(409, obj?.StatusCode);
        }

        [Fact]
        public async void Put_User_ReturnsBadRequest()
        {
            //Arrange
            var users = Fixtures.UsersFixture.GetTestUsers();
            var response = new ServiceResponse<User>
            {
                Data = users[0]
            };
            _userService.Setup(x => x.PutUser(users[1].Id, users[0])).ReturnsAsync(response);
            var controller = new UsersController(_userService.Object);

            //Act
            var result = await controller.PutUser(users[1].Id, users[0]);
            var obj = result as BadRequestResult;

            //Assert
            Assert.Equal(400, obj?.StatusCode);
        }

        [Fact]
        public async void Put_User_ReturnsNoContent()
        {
            //Arrange
            var users = Fixtures.UsersFixture.GetTestUsers();
            users[1].FirstName = "test";
            var response = new ServiceResponse<User>
            {
                Data = users[1]
            };
            _userService.Setup(x => x.PutUser(users[1].Id, users[1])).ReturnsAsync(response);
            var controller = new UsersController(_userService.Object);

            //Act
            var result = await controller.PutUser(users[1].Id, users[1]);
            var obj = result as NoContentResult;

            //Assert
            Assert.Equal(204, obj?.StatusCode);
        }

        [Fact]
        public async void Put_User_ReturnsNotFound()
        {
            //Arrange
            var users = Fixtures.UsersFixture.GetTestUsers();
            var response = new ServiceResponse<User>
            {
                Data = users[0],
                Success = false
            };
            _userService.Setup(x => x.PutUser(users[0].Id, users[0])).ReturnsAsync(response);
            var controller = new UsersController(_userService.Object);

            //Act
            var result = await controller.PutUser(users[0].Id, users[0]);
            var obj = result as NotFoundResult;

            //Assert
            Assert.Equal(404, obj?.StatusCode);
        }

        [Fact]
        public async void Delete_User_ReturnsNotFound()
        {
            //Arrange
            var users = Fixtures.UsersFixture.GetTestUsers();
            var response = new ServiceResponse<User>
            {
                Data = null,
                Success = false,
                Message = "NotFound"
            };
            _userService.Setup(x => x.DeleteUser(1)).ReturnsAsync(response);
            var controller = new UsersController(_userService.Object);

            //Act
            var result = await controller.DeleteUser(1);
            var obj = result as NotFoundResult;

            //Assert
            Assert.Equal(404, obj?.StatusCode);
        }

        [Fact]
        public async void Delete_User_ReturnsNoContent()
        {
            //Arrange
            var users = Fixtures.UsersFixture.GetTestUsers();
            var response = new ServiceResponse<User>
            {
                Data = users[0]
            };
            _userService.Setup(x => x.DeleteUser(1)).ReturnsAsync(response);
            var controller = new UsersController(_userService.Object);

            //Act
            var result = await controller.DeleteUser(1);
            var obj = result as NoContentResult;

            //Assert
            Assert.Equal(204, obj?.StatusCode);
        }

        //private List<User> GetUsers()
        //{
        //    List<User> users = new List<User>
        //    {
        //        new User
        //        {
        //            Id=1,
        //            FirstName="Peter",
        //            LastName="Parker",
        //            Email="peter.parker@gmail.com",
        //            Password="Asdf1234"
        //        },
        //        new User
        //        {
        //            Id=2,
        //            FirstName="Tony",
        //            LastName="Stark",
        //            Email="tony.stark@gmail.com",
        //            Password="Qwerty123"
        //        }
        //    };

        //    return users;
        //}
    }
}
