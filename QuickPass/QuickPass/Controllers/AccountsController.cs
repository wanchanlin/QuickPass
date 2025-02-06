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
    public class AccountsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AccountsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all accounts from the database
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccounts()
        {
            return await _context.Accounts.ToListAsync();
        }

        /// <summary>
        /// Retrieves a specific account by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetAccount(int id)
        {
            var account = await _context.Accounts.FindAsync(id);

            if (account == null)
            {
                return NotFound();
            }

            return account;
        }

        /// <summary>
        /// Updates a specific account by ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccount(int id, Account account)
        {
            if (id != account.AccountId)
            {
                return BadRequest();
            }

            _context.Entry(account).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        ///  Creates a new account
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Account>> PostAccount(Account account)
        {
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAccount", new { id = account.AccountId }, account);
        }

        /// <summary>
        ///  Deletes a specific account by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound();
            }

            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        /// <summary>
        /// Links an event to an account and creates a ticket for the event
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="eventId"></param>
        /// <returns></returns>
        [HttpPost("LinkEvent")]
        public async Task<IActionResult> LinkEvent(int accountId, int eventId)
        {
            var account = await _context.Accounts
                .Include(a => a.Events)
                .FirstOrDefaultAsync(a => a.AccountId == accountId);
            if (account == null)
            {
                return NotFound();
            }

            var eventEntity = await _context.Events.FindAsync(eventId);
            if (eventEntity == null)
            {
                return NotFound();
            }

            if (!account.Events.Contains(eventEntity))
            {
                account.Events.Add(eventEntity);

                // Create a new ticket for the account and event
                var ticket = new Ticket
                {
                    AccountId = accountId,
                    EventId = eventId,
                    Price = 0, // Set the appropriate price
                    SeatNumber = "N/A", // Set the appropriate seat number
                    BookingDate = DateTime.Now
                };
                _context.Tickets.Add(ticket);

                await _context.SaveChangesAsync();
            }

            return NoContent();
        }
        /// <summary>
        /// DELETE: api/Accounts/UnlinkEvent
        /// </summary>DELETE: api/Accounts/UnlinkEvent
        /// Unlinks an event from an account and removes the associated ticket
        /// <param name="accountId"></param>
        /// <param name="eventId"></param>
        /// <returns></returns>

        [HttpDelete("UnlinkEvent")]
        public async Task<IActionResult> UnlinkEvent(int accountId, int eventId)
        {
            var account = await _context.Accounts
                .Include(a => a.Events)
                .FirstOrDefaultAsync(a => a.AccountId == accountId);
            if (account == null)
            {
                return NotFound();
            }

            var eventEntity = account.Events.FirstOrDefault(e => e.EventId == eventId);
            if (eventEntity == null)
            {
                return NotFound();
            }

            account.Events.Remove(eventEntity);

            // Remove the ticket for the account and event
            var ticket = await _context.Tickets
                .FirstOrDefaultAsync(t => t.AccountId == accountId && t.EventId == eventId);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }
        /// <summary>
        /// Checks if an account exists by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool AccountExists(int id)
        {
            return _context.Accounts.Any(e => e.AccountId == id);
        }
    }
}
