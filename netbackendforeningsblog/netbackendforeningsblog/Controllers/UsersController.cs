namespace netbackendforeningsblog.ControllersAuth;

using Microsoft.AspNetCore.Mvc;
using netbackendforeningsblog.Models;
using netbackendforeningsblog.Authorization;
using netbackendforeningsblog.Models.Users;
using netbackendforeningsblog.Services;

[Authorize]
[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    //[HttpPost]
    //public IActionResult Register(User model)
    //{
    //    var response = _userService.Register(model);
    //    return Ok(response);
    //}

    [AllowAnonymous]
    [HttpPost("[action]")]
    public IActionResult Authenticate(AuthenticateRequest model)
    {
        var response = _userService.Authenticate(model);
        return Ok(response);
    }

    [Authorize(Role.Admin)]
    [HttpGet]
    public IActionResult GetAll()
    {
        var users = _userService.GetAll();
        return Ok(users);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        // only admins can access other user records
        var currentUser = (User)HttpContext.Items["User"];
        if (id != currentUser.Id && currentUser.Role != Role.Admin)
            return Unauthorized(new { message = "Unauthorized" });

        var user =  _userService.GetById(id);
        return Ok(user);
    }

    
}