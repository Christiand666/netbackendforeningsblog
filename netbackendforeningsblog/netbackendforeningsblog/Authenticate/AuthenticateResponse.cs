namespace netbackendforeningsblog.Models.Users;
using netbackendforeningsblog.Models;


public class AuthenticateResponse
{
    //returns the data by a succesful authenticate from userscontroll contains details from the user entities and the jwt token
    public int Id { get; set; }
    public string FullName { get; set; } 
    public string Email { get; set; } 
    public Role Role { get; set; }
    public string Token { get; set; }

    public AuthenticateResponse(User user, string token)
    {
        Id = user.Id;
        FullName = user.FullName;
        Email = user.Email;
        Role = user.Role;
        Token = token;
    }
}