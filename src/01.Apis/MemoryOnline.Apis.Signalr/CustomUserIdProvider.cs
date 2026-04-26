using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace MemoryOnline.Apis.Signalr
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public virtual string? GetUserId(HubConnectionContext connection)
        {
            // ASP.NET Core remapea "sub" -> ClaimTypes.NameIdentifier automáticamente
            //vamos a utilizar el claim "sub" para obtener el userId, ya que es el claim estándar para identificar al usuario en sistemas de autenticación basados en tokens (como JWT).
            var userId = connection.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return userId;
        }
    }
}
