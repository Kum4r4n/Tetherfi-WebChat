﻿using Signal.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Signal.Application.Interfaces.Repository
{
    public interface IUserDataRepository
    {
        Task<UserDataModel> GetUserData(Guid userId);
    }
}
