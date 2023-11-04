using Identity.Application.Models.Common;
using Identity.Application.Models.Request;
using Identity.Application.Models.Response;
using System.Security.Claims;

namespace Identity.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<TokenModel> RegisterUser(RegisterRequestModel registerRequestModel);
        Task<UserResponseModel> Update(UpdateUserRequestModel updateUserRequestModel, ClaimsPrincipal user);
        Task<UserResponseModel> GetUser(ClaimsPrincipal user);
        Task<TokenModel> Login(LoginRequestModel loginRequestModel);
    }
}
