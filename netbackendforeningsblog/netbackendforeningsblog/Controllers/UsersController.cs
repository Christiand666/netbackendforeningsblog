namespace netbackendforeningsblog.ControllersAuth;

using Microsoft.AspNetCore.Mvc;
using netbackendforeningsblog.Models;
using netbackendforeningsblog.Authorization;
using netbackendforeningsblog.Models.Users;
using netbackendforeningsblog.Services;
using netbackendforeningsblog.DAL;
using BCryptNet = BCrypt.Net.BCrypt;


[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private IUserService _userService;
    private readonly ForeningsblogContext _context;

    public UsersController(IUserService userService, ForeningsblogContext context)
    {
        _userService = userService;
        _context = context;
    }

    [HttpPost("Register")]
    public IActionResult Register([FromBody] User model)
    {
        var user = _context.Users.SingleOrDefault(u => u.Email == model.Email);
        if (user != null)
        {
            return BadRequest(new JsonResult(new { message = "Whoops brugeren eksistere" }));
        }

        var testUsers = new User { Email = model.Email, Password = model.Password, FullName = model.FullName, PasswordHash = BCryptNet.HashPassword(model.Password), Role = Role.User };


        _context.Users.AddRange(testUsers);
        _context.SaveChanges();
        return Ok(testUsers);

    }


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

        var user = _userService.GetById(id);
        return Ok(user);
    }

    [Authorize(Role.Admin)]
    [HttpDelete("DeleteUser")]
    public IActionResult DeleteUser([FromBody] int id)
    {
        var users = _userService.Deleteuser(id);
        return Ok(users);

    }


}