#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using netbackendforeningsblog.DAL;
using netbackendforeningsblog.Models;
using BCrypt.Net;
using Microsoft.Extensions.Options;
using netbackendforeningsblog.Authorization;
using netbackendforeningsblog.Helpers;
using netbackendforeningsblog.Models.Users;
using netbackendforeningsblog.Services;
using BCryptNet = BCrypt.Net.BCrypt;

namespace netbackendforeningsblog.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly ForeningsblogContext _context;

        public UsersController(ForeningsblogContext context)
        {
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

            var testUsers = new User{ Email = model.Email, Password = model.Password, FullName = model.FullName, PasswordHash = BCryptNet.HashPassword(model.Password), Role = Role.User };

           
            _context.Users.AddRange(testUsers);
            _context.SaveChanges();
            return Ok(testUsers);

        }
    }
}
