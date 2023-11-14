using Grpc.Core;
using Identity.Application.Interfaces.Repositories;
using Identity.Application.Models.Response;
using Identity.Infrastructure.Proto;
using System.Text.Json;

namespace Identity.Infrastructure.Providers
{
    public class UserGrpcProvider : UserService.UserServiceBase
    {
        private readonly IUserRepository _userRepository;

        public UserGrpcProvider(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public override async Task<UserResponse> GetUsersNames(UserRequest request, ServerCallContext context)
        {
            var data = await _userRepository.GetUserNames(request.ReqMessage.Split(',').Select(s => Guid.Parse(s)).ToList());
            return new UserResponse()
            {
                Message = JsonSerializer.Serialize(data.Select(s => new ProtoUserModel() { Id = s.Id, Name = s.Name }).ToList())
            };
        }
    }
}
