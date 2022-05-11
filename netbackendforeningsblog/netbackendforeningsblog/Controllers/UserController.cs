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
        


        // GET: Users
        [HttpGet]
        public async Task<ActionResult<List<Models.User>>> Get()
        {
            return await _context.Users.ToListAsync();
        }

        [HttpPost("Register")]
        public IActionResult Register(User model)
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
            return Ok(testUsers);

        }

        [HttpGet("{id?}")]
        public async Task<ActionResult<Models.User>> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost(Name = "Create")]
        public async Task<IActionResult> Create([Bind("UserRole,Email,Password,FullName")] Models.User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (string.IsNullOrEmpty(user.Email) ||
                        string.IsNullOrEmpty(user.Password) ||
                        string.IsNullOrEmpty(user.FullName))
                        return BadRequest();

                    _context.Add(user);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction(nameof(Details), new { id = user.Id }, user);
                }

                return BadRequest();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserRole,Email,Password,FullName")] Models.User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Get));
            }
            return NoContent();
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Get));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
