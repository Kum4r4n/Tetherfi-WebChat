using Identity.Application.Interfaces.Repositories;
using Identity.Application.Interfaces.Services;
using Identity.Application.Models.Request;
using Identity.Application.Services;
using Identity.Domain.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Identity.xUnit
{
    public class UserServiceTest
    {
        private IUserService _userService;
        private readonly Mock<IUserRepository> _mockUserRepository = new Mock<IUserRepository>();
        private readonly Mock<IPasswordService> _mockePasswordService = new Mock<IPasswordService>();
        private readonly Mock<ITokenService> _mockTokenService = new Mock<ITokenService>();


        public UserServiceTest()
        {
            _userService = new UserService(_mockUserRepository.Object, _mockePasswordService.Object, _mockTokenService.Object);
        }

        [Fact]
        public async Task RegisterTest()
        {

            var requestModel = new Mock<RegisterRequestModel>();

            _mockUserRepository.Setup(s => s.Get(It.IsAny<string>())).ReturnsAsync(value: null);
            _mockePasswordService.Setup(s => s.Hash(It.IsAny<string>())).Returns("hashed_password");
            _mockTokenService.Setup(s => s.GenerateAccessToken(It.IsAny<List<Claim>>())).Returns("Token");

            _mockUserRepository.Setup(s => s.Add(It.IsAny<User>())).ReturnsAsync(new User
            {

                Email = "sample@gmail.com",
                Password = "<PASSWORD>",
                Id = Guid.Parse("58BD84F8-720A-4B05-A8A0-C475259FE687"),
                Name = "Test",

            });

            var result = await _userService.RegisterUser(requestModel.Object);

            Assert.True(result != null && !string.IsNullOrEmpty(result.Token));

        }

        [Fact]
        public async Task GetUseTest()
        {
            _mockUserRepository.Setup(s => s.Get(It.IsAny<Guid>())).ReturnsAsync(value : new User
            {
                Email = "sample@gmail.com",
                Password = "<PASSWORD>",
                Id = Guid.Parse("58BD84F8-720A-4B05-A8A0-C475259FE687"),
                Name = "Test",
            });

            var claims = new List<Claim>() {
                new Claim(ClaimTypes.Name, "58BD84F8-720A-4B05-A8A0-C475259FE687")
            };

            var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, "TestAuthType"));

            var result = await _userService.GetUser(userPrincipal);

            Assert.True(result != null && result.Email == "sample@gmail.com");
        }
    }
}
