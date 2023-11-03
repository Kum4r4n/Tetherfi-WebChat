using Identity.Application.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<string> RegisterUser(RegisterRequestModel registerRequestModel);
    }
}
