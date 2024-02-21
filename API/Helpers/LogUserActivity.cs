using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Helpers;

// en BaseApiController pongo la anotation [ServiceFilter(typeof(LogUserActivity))]
// pongo services.AddScoped<LogUserActivity>(); en ApplicationServiceExtension.cs
public class LogUserActivity : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // voy a ejecutar ___ despues de q el user haya hecho lo suyo ( cuando la accion en la api se halla completado )
        var resultContext = await next();

        // es true si el token q manda se pudo autenticar
        if (!resultContext.HttpContext.User.Identity.IsAuthenticated) return;

        var userId = resultContext.HttpContext.User.GetUserId();

        var uow = resultContext.HttpContext.RequestServices.GetRequiredService<IUnitOfWork>();
        var user = await uow.UserRepository.GetUserByIdAsync(userId);

        user.LastActive = DateTime.UtcNow;
        await uow.Complete();
    }
}
