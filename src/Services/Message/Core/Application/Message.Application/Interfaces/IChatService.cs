using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Message.Application.Interfaces
{
    public interface IChatService
    {
        Task ReceiveMessage(ClaimsPrincipal user, string message, Guid partnerId);
    }
}
