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

namespace netbackendforeningsblog.Controllers
{
    [Route("api/[controller]")]
    public class BlogsController : Controller
    {
        private readonly ForeningsblogContext _context;

        public BlogsController(ForeningsblogContext context)
        {
            _context = context;
        }

        // GET: Blogs
        [HttpGet]
        public async Task<ActionResult<List<Blog>>> Get()
        {
            var foreningsblogContext = _context.Blogs.Include(b => b.User);
            return await foreningsblogContext.ToListAsync();
        }

        [HttpGet("{id?}")]
        public async Task<ActionResult<Blog>> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _context.Blogs
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blog == null)
            {
                return NotFound();
            }

            return blog;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,UserId,Author")] Blog blog)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(blog);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction(nameof(Details), new { id = blog.Id }, blog);
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,UserId,Author")] Blog blog)
        {
            if (id != blog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(blog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogExists(blog.Id))
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

        [HttpDelete("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blog = await _context.Blogs.FindAsync(id);
            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Get));
        }

        private bool BlogExists(int id)
        {
            return _context.Blogs.Any(e => e.Id == id);
        }
    }
}
