using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuickPass.Data;
using QuickPass.Models;

namespace QuickPass.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Constructor to initialize the database context.
        /// </summary>
        /// <param name="context">Application database context.</param>
        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all events from the database.
        /// </summary>
        /// <returns>A list of all events.</returns>
        /// <response code="200">Returns the list of events.</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
        {
            return Ok(await _context.Events.ToListAsync());
        }

        /// <summary>
        /// Retrieves a specific event by ID.
        /// </summary>
        /// <param name="id">The unique identifier of the event.</param>
        /// <returns>Returns the event details or a 404 Not Found if the event does not exist.</returns>
        /// <response code="200">Returns the requested event.</response>
        /// <response code="404">Event not found.</response>
        /// <example>GET: api/events/5</example>
        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEvent(int id)
        {
            var @event = await _context.Events.FindAsync(id);

            if (@event == null)
            {
                return NotFound("Event not found.");
            }

            return Ok(@event);
        }

        /// <summary>
        /// Updates a specific event by ID.
        /// </summary>
        /// <param name="id">The ID of the event to update.</param>
        /// <param name="event">The updated event object.</param>
        /// <returns>Returns 204 No Content if successful, 400 Bad Request if the IDs do not match, or 404 Not Found if the event does not exist.</returns>
        /// <response code="204">Event updated successfully.</response>
        /// <response code="400">Bad request, event ID mismatch.</response>
        /// <response code="404">Event not found.</response>
        /// <example>PUT: api/events/5</example>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvent(int id, Event @event)
        {
            if (id != @event.EventId)
            {
                return BadRequest("Event ID mismatch.");
            }

            _context.Entry(@event).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(id))
                {
                    return NotFound("Event not found.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Creates a new event.
        /// </summary>
        /// <param name="event">The event object to be created.</param>
        /// <returns>Returns the created event with a 201 Created status.</returns>
        /// <response code="201">Event created successfully.</response>
        /// <response code="400">Bad request, invalid input.</response>
        /// <example>POST: api/events</example>
        [HttpPost]
        public async Task<ActionResult<Event>> PostEvent(Event @event)
        {
            if (@event == null)
            {
                return BadRequest("Invalid event data.");
            }

            _context.Events.Add(@event);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEvent", new { id = @event.EventId }, @event);
        }

        /// <summary>
        /// Deletes a specific event by ID.
        /// </summary>
        /// <param name="id">The ID of the event to delete.</param>
        /// <returns>Returns 204 No Content if successful, or 404 Not Found if the event does not exist.</returns>
        /// <response code="204">Event deleted successfully.</response>
        /// <response code="404">Event not found.</response>
        /// <example>DELETE: api/events/5</example>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound("Event not found.");
            }

            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Retrieves a list of events with their related ticket details.
        /// </summary>
        /// <returns>A list of events including ticket information.</returns>
        /// <response code="200">Returns a list of events with ticket details.</response>
        /// <example>GET: api/events/ListEventsDTO</example>
        [HttpGet(template: "ListEventsDTO")]
        public async Task<ActionResult<IEnumerable<Event>>> ListEventsDTO()
        {
            var events = await _context.Events
                .Include(e => e.Tickets)
                .ThenInclude(t => t.Account)
                .ToListAsync();

            if (events == null || !events.Any())
            {
                return NotFound("No events found.");
            }

            var eventDtos = events.Select(@event => new Event
            {
                EventId = @event.EventId,
                Name = @event.Name,
                Description = @event.Description,
                Venue = @event.Venue,
                Date = @event.Date,
                TotalTickets = @event.TotalTickets,
                Tickets = @event.Tickets.Select(ticket => new Ticket
                {
                    TicketId = ticket.TicketId,
                    Price = ticket.Price,
                    SeatNumber = ticket.SeatNumber,
                    BookingDate = ticket.BookingDate,
                    AccountId = ticket.AccountId,
                }).ToList()
            }).ToList();

            return Ok(eventDtos);
        }

        /// <summary>
        /// Checks if an event exists by ID.
        /// </summary>
        /// <param name="id">The ID of the event.</param>
        /// <returns>Returns true if the event exists, false otherwise.</returns>
        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.EventId == id);
        }
    }
}
