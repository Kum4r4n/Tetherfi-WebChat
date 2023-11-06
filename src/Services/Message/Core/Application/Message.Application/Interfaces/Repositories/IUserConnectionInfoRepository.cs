﻿using Message.Application.Models;
using Message.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Message.Application.Interfaces.Repositories
{
    public interface IUserConnectionInfoRepository
    {
        Task<bool> AddUpdate(Guid id, string connectionId);
        Task Remove(Guid id);
        Task<List<ChatUserModel>> GetAllUsersExceptThis(Guid id);
        Task<UserConnectionInfo> GetUserInfo(Guid id);
    }
}
