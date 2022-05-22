namespace netbackendforeningsblog.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using netbackendforeningsblog.Models;


[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    //list of all the roles, role is specified admin or just logged in
    private readonly IList<Role> _roles;

    public AuthorizeAttribute(params Role[] roles)
    {
        _roles = roles ?? new Role[] { };
    }

    //gets here if a attribute is declared [Authorize] or [Authorize.admin]
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // jumps over the if the actions is decorated with allowanonymous attributen
        var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
        if (allowAnonymous)
            return;

        // authorization that the user is in an authorized role (if specified).
        var user = (User)context.HttpContext.Items["User"];
        if (user == null || (_roles.Any() && !_roles.Contains(user.Role)))
        {
            // if the user is not logged in or not authorized 
            //401 + unauthorized if either there is not allowanonymous or is retricted to admin 
            context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
        }
    }
}