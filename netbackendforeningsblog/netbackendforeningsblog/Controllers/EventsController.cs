#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using netbackendforeningsblog.Authorization;
using netbackendforeningsblog.DAL;
using netbackendforeningsblog.Models;
using Newtonsoft.Json.Linq;


namespace netbackendforeningsblog.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly ForeningsblogContext _context;
        

        public EventsController(ForeningsblogContext context)
        {
            _context = context;
        }

        // GET: Events
        [HttpGet]
        public async Task<ActionResult<List<Event>>> Get()
        {
            return await _context.Events.ToListAsync();
        }

        [HttpGet("{id?}")]
        public async Task<ActionResult<Event>> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            return @event;
        }

        [HttpPost("Attend/{eventId}/{userId}")]
        [Authorize]
        public async Task<IActionResult> Attend([FromBody]int eventId, [FromBody]int userId)
        {
            try
            {
                if (eventId > 0 && userId > 0)
                {
                    var alreadyExists = _context.SignedupUsers.Any(u => u.Id == userId && u.EventId == eventId);

                    if (alreadyExists)
                    {
                        _context.SignedupUsers.Add(new SignedupUsers { UserId = userId, EventId = eventId });
                        await _context.SaveChangesAsync();
                    }

                    return Ok();
                }

                return BadRequest(new JsonResult(new { message = "Invalid forespørgelse" }));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Role.Admin)]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Event @event)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(@event);
                    await _context.SaveChangesAsync();
                    return CreatedAtAction(nameof(Details), new { id = @event.Id }, @event);
                }

                return BadRequest();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Role.Admin)]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,Place,CreatedDateTime,StartDateTime,EndDateTime")] Event @event)
        {
            if (id != @event.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@event);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!EventExists(@event.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        var response = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                        response.Content = new StringContent(ex.Message);
                        return (IActionResult)Task.FromResult(response);
                    }
                }
                return RedirectToAction(nameof(Get));
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Role.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}
