namespace netbackendforeningsblog.Models.Users;

using System.ComponentModel.DataAnnotations;

public class AuthenticateRequest
{
    // required field returns to the api if it is false
  [Required]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}