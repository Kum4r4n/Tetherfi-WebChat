using Identity.Application.Models.Request;
using Identity.Application.Models.Response;
using System.Security.Claims;

namespace Identity.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<string> RegisterUser(RegisterRequestModel registerRequestModel);
        Task<UserResponseModel> Update(UpdateUserRequestModel updateUserRequestModel, ClaimsPrincipal user);
        Task<UserResponseModel> GetUser(ClaimsPrincipal user);
        Task<string> Login(LoginRequestModel loginRequestModel);
    }
}
