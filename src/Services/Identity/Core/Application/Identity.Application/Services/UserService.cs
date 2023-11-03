using Identity.Application.Exceptions;
using Identity.Application.Interfaces.Repositories;
using Identity.Application.Interfaces.Services;
using Identity.Application.Models.Request;
using Identity.Domain.Entities;
using System.Security.Claims;

namespace Identity.Application.Services
{
    public class UserService
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

        public async Task RegisterUser(RegisterRequestModel registerRequestModel)
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
        }
    }
}
