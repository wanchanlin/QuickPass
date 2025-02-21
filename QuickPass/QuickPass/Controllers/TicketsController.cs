
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuickPass.Data;
using QuickPass.Models;

namespace QuickPass.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Constructor to initialize the database context.
        /// </summary>
        /// <param name="context">Application database context.</param>
        public TicketsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all tickets from the database.
        /// </summary>
        /// <returns>A list of all tickets.</returns>
        /// <response code="200">Returns the list of tickets.</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTickets()
        {
            return Ok(await _context.Tickets.ToListAsync());
        }

        /// <summary>
        /// Retrieves a specific ticket by ID.
        /// </summary>
        /// <param name="id">The unique identifier of the ticket.</param>
        /// <returns>Returns the ticket details or 404 Not Found if the ticket does not exist.</returns>
        /// <response code="200">Returns the requested ticket.</response>
        /// <response code="404">Ticket not found.</response>
        /// <example>GET: api/tickets/5</example>
        [HttpGet("{id}")]
        public async Task<ActionResult<Ticket>> GetTicket(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);

            if (ticket == null)
            {
                return NotFound("Ticket not found.");
            }

            return Ok(ticket);
        }

        /// <summary>
        /// Updates a specific ticket by ID.
        /// </summary>
        /// <param name="id">The ID of the ticket to update.</param>
        /// <param name="ticket">The updated ticket object.</param>
        /// <returns>Returns 204 No Content if successful, 400 Bad Request if the IDs do not match, or 404 Not Found if the ticket does not exist.</returns>
        /// <response code="204">Ticket updated successfully.</response>
        /// <response code="400">Bad request, ticket ID mismatch.</response>
        /// <response code="404">Ticket not found.</response>
        /// <example>PUT: api/tickets/5</example>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTicket(int id, Ticket ticket)
        {
            if (id != ticket.TicketId)
            {
                return BadRequest("Ticket ID mismatch.");
            }

            _context.Entry(ticket).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TicketExists(id))
                {
                    return NotFound("Ticket not found.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Creates a new ticket.
        /// </summary>
        /// <param name="ticket">The ticket object to be created.</param>
        /// <returns>Returns the created ticket with a 201 Created status.</returns>
        /// <response code="201">Ticket created successfully.</response>
        /// <response code="400">Bad request, invalid input.</response>
        /// <example>POST: api/tickets</example>
        [HttpPost]
        public async Task<ActionResult<Ticket>> PostTicket(Ticket ticket)
        {
            if (ticket == null)
            {
                return BadRequest("Invalid ticket data.");
            }

            // Set navigation properties to null to prevent validation errors
            ticket.Event = null;
            ticket.Account = null;

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTicket", new { id = ticket.TicketId }, ticket);
        }


        /// <summary>
        /// Deletes a specific ticket by ID.
        /// </summary>
        /// <param name="id">The ID of the ticket to delete.</param>
        /// <returns>Returns 204 No Content if successful, or 404 Not Found if the ticket does not exist.</returns>
        /// <response code="204">Ticket deleted successfully.</response>
        /// <response code="404">Ticket not found.</response>
        /// <example>DELETE: api/tickets/5</example>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound("Ticket not found.");
            }

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Checks if a ticket exists by ID.
        /// </summary>
        /// <param name="id">The ID of the ticket.</param>
        /// <returns>Returns true if the ticket exists, false otherwise.</returns>
        private bool TicketExists(int id)
        {
            return _context.Tickets.Any(e => e.TicketId == id);
        }
    }
}
