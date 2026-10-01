using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace MemoryOnline.Apis.Signalr.Filters
{
    public class GameExceptionFilter : IHubFilter
    {
        public async ValueTask<object?> InvokeMethodAsync(
            HubInvocationContext invocationContext, 
            Func<HubInvocationContext, ValueTask<object?>> next)
        {
            try
            {
                return await next(invocationContext);
            }
            catch (Exception ex)
            {
                // Envía el error al cliente que hizo la llamada tal y como lo hacías en el hub
                await invocationContext.Hub.Clients.Caller.SendAsync("Error", ex.Message);
                
                // En caso de que el método espere un retorno de valor, regresará el tipo por defecto (null)
                return null;
            }
        }
    }
}
