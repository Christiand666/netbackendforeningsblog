namespace netbackendforeningsblog.Services;
using BCrypt.Net;
using Microsoft.Extensions.Options;
using netbackendforeningsblog.DAL;
using netbackendforeningsblog.Models;
using netbackendforeningsblog.Authorization;
using netbackendforeningsblog.Helpers;
using netbackendforeningsblog.Models.Users;

//Contains methods from UserService into an interface
public interface IUserService
{
    User Deleteuser(int id);
    AuthenticateResponse Authenticate(AuthenticateRequest model);
    IEnumerable<User> GetAll();
    User GetById(int id);

    User Register(User TestUser);
}

public class UserService : IUserService
{

    private ForeningsblogContext _context;
    private IJwtUtils _jwtUtils;
    private readonly AppSettings _appSettings;

    public UserService(
        ForeningsblogContext context,
        IJwtUtils jwtUtils,
        IOptions<AppSettings> appSettings)
    {
        _context = context;
        _jwtUtils = jwtUtils;
        _appSettings = appSettings.Value;
    }
    

    public AuthenticateResponse Authenticate(AuthenticateRequest model)
    {
        var user = _context.Users.SingleOrDefault(x => x.Email == model.Email);

        // validate if user exist and the hashed password.
        if (user == null || !BCrypt.Verify(model.Password, user.PasswordHash))
            throw new AppException("Din mail eller password var forkert");

        // If authentication was succesful generate a JWT 
        var jwtToken = _jwtUtils.GenerateJwtToken(user);


        return new AuthenticateResponse(user, jwtToken);
    }
    //Returns all the users
    public IEnumerable<User> GetAll()
    {
        return _context.Users;
    }
    //Get specific User in Url (not used)
    public User GetById(int id) 
    {
        var user = _context.Users.Find(id);
        if (user == null) throw new KeyNotFoundException("User not found");
        return user;
    }

    //Deletes the user
    public User Deleteuser(int id)
    {
        var user = _context.Users.Find(id);
        if (user == null) throw new KeyNotFoundException("User not found");
        _context.Users.Remove(user);
        _context.SaveChanges();
        return user;
    }

    public User Register(User TestUser)
    {
        _context.Users.AddRange(TestUser);
        _context.SaveChanges();
        return TestUser;
    }
}