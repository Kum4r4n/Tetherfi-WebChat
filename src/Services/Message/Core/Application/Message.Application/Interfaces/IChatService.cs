using System.Security.Claims;

namespace Message.Application.Interfaces
{
    public interface IChatService
    {
        Task ReceiveMessage(ClaimsPrincipal user, string message, Guid partnerId);
    }
}
