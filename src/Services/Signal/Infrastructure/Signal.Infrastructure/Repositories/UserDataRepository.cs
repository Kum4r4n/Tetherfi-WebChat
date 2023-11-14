using Signal.Application.Interfaces.Repository;
using Signal.Application.Models;
using Signal.Infrastructure.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Signal.Infrastructure.Repositories
{
    public class UserDataRepository : IUserDataRepository
    {
        private readonly UserDataGrpcProvider _userDataGrpcProvider;

        public UserDataRepository(UserDataGrpcProvider userDataGrpcProvider)
        {
            _userDataGrpcProvider = userDataGrpcProvider;
        }

        public async Task<UserDataModel> GetUserData(Guid userId)
        {
            var data = await _userDataGrpcProvider.GetUserData(userId);
            return data;
        }
    }
}
