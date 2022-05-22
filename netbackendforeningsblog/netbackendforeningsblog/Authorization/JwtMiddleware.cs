namespace netbackendforeningsblog.Authorization;

using Microsoft.Extensions.Options;
using netbackendforeningsblog.Helpers;
using netbackendforeningsblog.Services;

public class JwtMiddleware
{
    //Handles error handling for duplicated errors 
    //proces the http request if not null pproceed to the next request or function
    private readonly RequestDelegate _next;
    private readonly AppSettings _appSettings;

    public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
    {
        _next = next;
        _appSettings = appSettings.Value;
    }
    // exstracts the jwt from the request from the authorization-header and validates on succes it returns the userid from the is returned and the user object is attached to the httpcontext
    // if it fails it returns a 401 unauthorized and denies the user the previlliges (kicks the user out on the frontend)
    public async Task Invoke(HttpContext context, IUserService userService, IJwtUtils jwtUtils)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        var userId = jwtUtils.ValidateJwtToken(token);
        if (userId != null)
        {
            // attaches user to the context if succesfull on the jwt validation
            context.Items["User"] = userService.GetById(userId.Value);
        }

        await _next(context);
    }
}