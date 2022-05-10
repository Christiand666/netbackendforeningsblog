namespace netbackendforeningsblog.Services;
using BCrypt.Net;
using Microsoft.Extensions.Options;
using netbackendforeningsblog.DAL;
using netbackendforeningsblog.Models;
using netbackendforeningsblog.Authorization;
using netbackendforeningsblog.Helpers;
using netbackendforeningsblog.Models.Users;
using BCryptNet = BCrypt.Net.BCrypt;

public interface IUserService
{
    User Register(User model);
    AuthenticateResponse Authenticate(AuthenticateRequest model);
    IEnumerable<User> GetAll();
    User GetById(int id);
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
    public User Register(User model)
    {
        var user = _context.Users.Find(model.Email);
        if (user != null)
        {
            throw new KeyNotFoundException("Exist");
        }

        var testUsers = new User();

        new User { Email = model.Email, Password = model.Password, FullName = model.FullName, PasswordHash = BCryptNet.HashPassword(model.Password), Role = Role.User };

        
        _context.Users.AddRange(testUsers);
        _context.SaveChanges();
        return testUsers;
    }

    public AuthenticateResponse Authenticate(AuthenticateRequest model)
    {
        var user = _context.Users.SingleOrDefault(x => x.Email == model.Email);

        // validate
        if (user == null || !BCrypt.Verify(model.Password, user.PasswordHash))
            throw new AppException("Username or password is incorrect");

        // authentication var succesful genere jwt token
        var jwtToken = _jwtUtils.GenerateJwtToken(user);

        return new AuthenticateResponse(user, jwtToken);
    }

    public IEnumerable<User> GetAll()
    {
        return _context.Users;
    }

    public User GetById(int id) 
    {
        var user = _context.Users.Find(id);
        if (user == null) throw new KeyNotFoundException("User not found");
        return user;
    }
}