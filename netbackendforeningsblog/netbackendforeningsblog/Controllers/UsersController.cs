namespace netbackendforeningsblog.ControllersAuth;

using Microsoft.AspNetCore.Mvc;
using netbackendforeningsblog.Models;
using netbackendforeningsblog.Authorization;
using netbackendforeningsblog.Models.Users;
using netbackendforeningsblog.Services;
using netbackendforeningsblog.DAL;
using BCryptNet = BCrypt.Net.BCrypt;

//Indicates that a type and all derived types are used to serve HTTP API responses.
//Controllers decorated with this attribute are configured with features and behavior
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    //injected IUserservice
    private IUserService _userService;
    private readonly ForeningsblogContext _context;

    public UsersController(IUserService userService, ForeningsblogContext context)
    {
        _userService = userService;
        _context = context;
    }
    //routes
    //does not need authorization
    [AllowAnonymous]
    [HttpPost("Register")]
    public IActionResult Register([FromBody] User model)
    {
        //get credentiels from json body and saves it to the database
        try 
        { 
            var user = _context.Users.SingleOrDefault(u => u.Email == model.Email);
            if (user != null)
            {
                return BadRequest(new JsonResult(new { message = "Whoops brugeren eksistere" }));
            }

            var testUsers = new User { Email = model.Email, Password = model.Password, FullName = model.FullName, PasswordHash = BCryptNet.HashPassword(model.Password), Role = Role.User };


            _userService.Register(testUsers);
            return Ok(testUsers);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // overwriten any new action methods added to the controller will be secure
    // does not need authorization
    [AllowAnonymous]
    [HttpPost("[action]")]
    public IActionResult Authenticate(AuthenticateRequest model)
    {
        try 
        { 
            //calls the authenticate from userservice which is injected from IUserservice 
            var response = _userService.Authenticate(model);
            return Ok(response);
        }
        catch(Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    //resticted route
    [Authorize(Role.Admin)]
    [HttpGet]
    public IActionResult GetAll()
    {
        var users = _userService.GetAll();
        return Ok(users);
    }

    //not used
    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        try
        {
            //Exstra logic if we wanted to change this route for users to Delete themself so we can check not only based on a admin but the which user can delete it self
            var currentUser = (User)HttpContext.Items["User"];
            if(currentUser != null)
            {
                if (id != currentUser.Id && currentUser.Role != Role.Admin)
                    return Unauthorized(new { message = "Unauthorized" });

                var user = _userService.GetById(id);
                return Ok(user);
            }

            return Unauthorized(new { message = "Unauthorized" });

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    //resticted route 
    [Authorize(Role.Admin)]
    [HttpDelete("DeleteUser/{id}")]
    public IActionResult DeleteUser(int id)
    {
        try 
        { 
            //Exstra logic if we wanted to change this route for users to Delete themself so we can check not only based on a admin but the which user can delete it self
             var currentUser = (User)HttpContext.Items["User"];
             if(currentUser != null)
             {
                //calls the Delete from userservice which is injected from IUserservice 
                if (id != currentUser.Id && currentUser.Role != Role.Admin)
                return Unauthorized(new { message = "Unauthorized" });
                var users = _userService.Deleteuser(id);
                return Ok(users);
             }

            return Unauthorized(new { message = "Unauthorized" });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }   
       
    }

}