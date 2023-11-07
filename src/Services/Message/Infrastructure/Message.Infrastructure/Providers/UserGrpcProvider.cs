using Message.Application.Models;
using System.Text.Json;

namespace Message.Infrastructure.Providers
{
    public class UserGrpcProvider
    {
        private readonly UserService.UserServiceClient _client;

        public UserGrpcProvider(UserService.UserServiceClient client)
        {
            _client = client;
        }

        public async Task<List<ProtoUserModel>> GetUsersNames(List<Guid> userIds)
        {
            var request = new UserRequest()
            {
                ReqMessage = string.Join(',', userIds)
            };

            var response = await _client.GetUsersNamesAsync(request);

            var data = JsonSerializer.Deserialize<List<ProtoUserModel>>(response.Message);
            return data;

        }
    }
}
