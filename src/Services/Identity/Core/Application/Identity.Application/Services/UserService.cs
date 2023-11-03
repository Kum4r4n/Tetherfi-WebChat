using Identity.Application.Exceptions;
using Identity.Application.Interfaces.Repositories;
using Identity.Application.Interfaces.Services;
using Identity.Application.Models.Common;
using Identity.Application.Models.Request;
using Identity.Application.Models.Response;
using Identity.Domain.Entities;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Identity.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;
        private readonly ITokenService _tokenService;
        public UserService(IUserRepository userRepository, IPasswordService passwordService, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _tokenService = tokenService;
        }

        public async Task<string> RegisterUser(RegisterRequestModel registerRequestModel)
        {
            var userExist = await _userRepository.Get(registerRequestModel.Email);
            if (userExist != null)
            {
                throw new BadRequestException("Email is already registered with another user");
            }

            var user = new User();
            user.Email = registerRequestModel.Email;
            user.Name = registerRequestModel.Name;
            user.Password = _passwordService.Hash(registerRequestModel.Password);
            var data = await _userRepository.Add(user);

            var claims = new List<Claim>() {
                new Claim(ClaimTypes.Name, data.Id.ToString()),
                new Claim(ClaimTypes.Email, data.Email )
            };

            var token = _tokenService.GenerateAccessToken(claims);

            return token;
        }

        public async Task<string> Login(LoginRequestModel loginRequestModel)
        {
            var user = await _userRepository.Get(loginRequestModel.Email);
            if(user == null)
            {
                throw new BadRequestException("Email is not exist, please register the account");
            }


            var isVerified = _passwordService.Verify(loginRequestModel.Password, user.Password);
            if (!isVerified)
            {
                throw new BadRequestException("Email or Password incorrect");
            }

            var claims = new List<Claim>() {
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email )
            };

            var token = _tokenService.GenerateAccessToken(claims);

            return token;
        }


        public async Task<UserResponseModel> GetUser(ClaimsPrincipal user)
        {
            var userId = Guid.Parse(user.Identity.Name);
            var userData = await _userRepository.Get(userId);

            var responseModel = new UserResponseModel()
            {
                Id = userId,
                Name = userData.Name,
                Email = userData.Email,
            };

            return responseModel;
        }

        public async Task<UserResponseModel> Update(UpdateUserRequestModel updateUserRequestModel, ClaimsPrincipal user)
        {
            var userId = Guid.Parse(user.Identity.Name);
            var userData = await _userRepository.Get(userId);

            userData.Name = updateUserRequestModel.Name;

            var data = await _userRepository.Update(userData);

            var responseModel = new UserResponseModel()
            {
                Id = userId,
                Name = userData.Name,
                Email = userData.Email,
            };

            return responseModel;
        }
    }
}
