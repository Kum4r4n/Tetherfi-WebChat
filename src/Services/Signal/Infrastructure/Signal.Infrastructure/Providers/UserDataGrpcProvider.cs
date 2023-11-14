using Signal.Application.Models;
using Signal.Infrastructure.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Signal.Infrastructure.Providers
{
    public class UserDataGrpcProvider 
    {
        private readonly UserDataService.UserDataServiceClient _client;

        public UserDataGrpcProvider(UserDataService.UserDataServiceClient client)
        {
            _client = client;
        }

        public async Task<UserDataModel> GetUserData(Guid userId)
        {
            var data =  _client.GetUserData(new UserDataRequest() { UserId = userId.ToString() });
            return JsonSerializer.Deserialize<UserDataModel>(data?.Data ?? "");
        }
    }
}
