using Grpc.Core;
using Message.Application.Interfaces.Repositories;
using Message.Infrastructure.Proto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Message.Infrastructure.Providers
{
    public class UserDataGrpcProvider :  UserDataService.UserDataServiceBase
    {
        private readonly IUserConnectionInfoRepository _userConIUserConnectionInfoRepository;

        public UserDataGrpcProvider(IUserConnectionInfoRepository userConIUserConnectionInfoRepository)
        {
            _userConIUserConnectionInfoRepository = userConIUserConnectionInfoRepository;
        }

        public override async Task<UserDataResponse> GetUserData(UserDataRequest request, ServerCallContext context)
        {
            var data = await _userConIUserConnectionInfoRepository.GetUserData(Guid.Parse(request.UserId));
            return new UserDataResponse()
            {
                Data = JsonSerializer.Serialize(data)
            };
        }
    }
}
