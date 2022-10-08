using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersNG.Controllers;
using UsersNG.DTO;
using UsersNG.Models;
using UsersNG.Services.AuthService;
using UsersNG.Shared;

namespace UsersNG.Tests.Controllers
{
    public class AuthControllerTests
    {
        private readonly Mock<IAuthService> _authService;
        public AuthControllerTests()
        {
            _authService = new Mock<IAuthService>();
        }

        [Fact]
        public async void Register_ReturnsConflict()
        {
            //Arrange
            var users = Fixtures.UsersFixture.GetTestUsers();
            var response = new ServiceResponse<User>
            {
                Success = false
            };
            
            var request = new UserDto();
            request.Email = users[0].Email;
            request.Password = users[0].Password;

            _authService.Setup(x => x.Register(request)).ReturnsAsync(response);
            var controller = new AuthController(_authService.Object);

            //Act
            var result = await controller.Register(request);
            var obj = result as ConflictObjectResult;

            //Assert
            Assert.Equal(409, obj?.StatusCode);
        }

        [Fact]
        public async void Register_ReturnsOk()
        {
            //Arrange
            var users = Fixtures.UsersFixture.GetTestUsers();
            var response = new ServiceResponse<User>
            {
                Data = users[0]
            };

            var request = new UserDto();
            request.Email = users[0].Email;
            request.Password = users[0].Password;

            _authService.Setup(x => x.Register(request)).ReturnsAsync(response);
            var controller = new AuthController(_authService.Object);

            //Act
            var result = await controller.Register(request);
            var obj = result as OkObjectResult;
            var data = obj?.Value as ServiceResponse<User>;

            //Assert
            Assert.Equal(200, obj?.StatusCode);
            Assert.Equal(response.Data.Id, data?.Data?.Id);
        }

        [Fact]
        public async void Login_ReturnsOk()
        {
            //Arrange
            var users = Fixtures.UsersFixture.GetTestUsers();
            var response = new ServiceResponse<string>
            {
                
            };

            var request = new LoginDto();
            request.Email = users[0].Email;
            request.Password = users[0].Password;

            _authService.Setup(x => x.Login(request)).ReturnsAsync(response);
            var controller = new AuthController(_authService.Object);

            //Act
            var result = await controller.Login(request);
            var obj = result as OkObjectResult;

            //Assert
            Assert.Equal(200, obj?.StatusCode);
        }

        [Fact]
        public async void Login_ReturnsBadRequest()
        {
            //Arrange
            var users = Fixtures.UsersFixture.GetTestUsers();
            var response = new ServiceResponse<string>
            {
                Success = false,
            };

            var request = new LoginDto();
            request.Email = users[0].Email;
            request.Password = users[1].Password;

            _authService.Setup(x => x.Login(request)).ReturnsAsync(response);
            var controller = new AuthController(_authService.Object);

            //Act
            var result = await controller.Login(request);
            var obj = result as BadRequestObjectResult;

            //Assert
            Assert.Equal(400, obj?.StatusCode);
        }
    }
}
