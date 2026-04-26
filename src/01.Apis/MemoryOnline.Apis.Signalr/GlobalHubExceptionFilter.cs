using Microsoft.AspNetCore.SignalR;


namespace MemoryOnline.Apis.Signalr
{
    public class GlobalHubExceptionFilter : IHubFilter
    {
        public async ValueTask<object?> InvokeMethodAsync(
            HubInvocationContext context,
            Func<HubInvocationContext, ValueTask<object?>> next)
        {
            try
            {
                return await next(context);
            }
            catch (Exception ex)
            {
                // Loguea el error
                Console.WriteLine($"[SignalR] Exception: {ex.Message}");
                // Puedes enviar un mensaje de error al cliente si tienes contexto
                await context.Hub.Clients.Caller.SendAsync("Error", "Error interno en el servidor.");
                throw; // Re-lanza para que SignalR lo maneje
            }
        }
    }
}
