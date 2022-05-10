namespace netbackendforeningsblog.Authorization;

using Microsoft.Extensions.Options;
using netbackendforeningsblog.Helpers;
using netbackendforeningsblog.Services;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly AppSettings _appSettings;

    public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
    {
        _next = next;
        _appSettings = appSettings.Value;
    }

    public async Task Invoke(HttpContext context, IUserService userService, IJwtUtils jwtUtils)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        var userId = jwtUtils.ValidateJwtToken(token);
        if (userId != null)
        {
            // knytter brugeren til kontekst ved vellykket jwt-validering
            context.Items["User"] = userService.GetById(userId.Value);
        }

        await _next(context);
    }
}