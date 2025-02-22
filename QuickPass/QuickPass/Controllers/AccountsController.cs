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

        /// <summary>
        /// Constructor to initialize the database context.
        /// </summary>
        /// <param name="context">Application database context.</param>
        public AccountsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all accounts from the database.
        /// </summary>
        /// <returns>A list of all accounts.</returns>
        /// <response code="200">Returns the list of accounts.</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccounts()
        {
            return Ok(await _context.Accounts.ToListAsync());
        }

        /// <summary>
        /// Retrieves a specific account by ID.
        /// </summary>
        /// <param name="id">The unique identifier of the account.</param>
        /// <returns>Returns the account details or 404 Not Found if the account does not exist.</returns>
        /// <response code="200">Returns the requested account.</response>
        /// <response code="404">Account not found.</response>
        /// <example>GET: api/accounts/5</example>
        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetAccount(int id)
        {
            var account = await _context.Accounts.FindAsync(id);

            if (account == null)
            {
                return NotFound("Account not found.");
            }

            return Ok(account);
        }
        /// <summary>
        /// Creates a new account.
        /// </summary>
        /// <param name="accountDto">The account object to be created.</param>
        /// <returns>Returns the created account with a 201 Created status.</returns>
        /// <response code="201">Account created successfully.</response>
        /// <response code="400">Bad request, invalid input.</response>
        /// <example>POST: api/accounts</example>
        [HttpPost]
        public async Task<ActionResult<Account>> CreateAccount(AccountDTO accountDto) // Use DTO
        {
            if (!ModelState.IsValid) // Validate model
            {
                return BadRequest(ModelState);
            }

            // Map DTO to Account entity
            var newAccount = new Account
            {
                FirstName = accountDto.FirstName,
                LastName = accountDto.LastName,
                Email = accountDto.Email,
                Password = accountDto.Password // Ensure DTO includes this
            };

            _context.Accounts.Add(newAccount);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAccount), new { id = newAccount.AccountId }, newAccount);
        }


        /// <summary>
        /// Retrieves a list of accounts with their related ticket details.
        /// </summary>
        /// <returns>A list of accounts including ticket information.</returns>
        /// <response code="200">Returns a list of accounts with ticket details.</response>
        /// <example>GET: api/accounts/DTO</example>
        [HttpGet("DTO")]
        public async Task<ActionResult<IEnumerable<AccountDTO>>> GetAccountsDTO()
        {
            var accounts = await _context.Accounts
                .Include(a => a.Tickets)
                .AsNoTracking()
                .ToListAsync();

            if (!accounts.Any())
            {
                return NotFound("No accounts found.");
            }

            var accountDtos = accounts.Select(account => new AccountDTO
            {
                AccountId = account.AccountId,
                FirstName = account.FirstName,
                LastName = account.LastName,
                Email = account.Email,
                Tickets = account.Tickets.Select(ticket => new TicketDTO
                {
                    TicketId = ticket.TicketId,
                    Price = ticket.Price,
                    SeatNumber = ticket.SeatNumber,
                    BookingDate = ticket.BookingDate,
                    EventId = ticket.EventId,
                }).ToList()
            }).ToList();

            return Ok(accountDtos);
        }

        /// <summary>
        /// Updates a specific account by ID.
        /// </summary>
        /// <param name="id">The ID of the account to update.</param>
        /// <param name="account">The updated account object.</param>
        /// <returns>Returns 204 No Content if successful, 400 Bad Request if the IDs do not match, or 404 Not Found if the account does not exist.</returns>
        /// <response code="204">Account updated successfully.</response>
        /// <response code="400">Bad request, account ID mismatch.</response>
        /// <response code="404">Account not found.</response>
        /// <example>PUT: api/accounts/5</example>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAccount(int id, Account account)
        {
            if (id != account.AccountId)
            {
                return BadRequest("Account ID mismatch.");
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
                    return NotFound("Account not found.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

       
        [HttpPut("Update/{id}")]
        public async Task<IActionResult> UpdateAccount(int id, AccountDTO accountDto) 
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != accountDto.AccountId)
                    return BadRequest("Account ID mismatch.");

                var existingAccount = await _context.Accounts.FindAsync(id);
                if (existingAccount == null)
                    return NotFound("Account not found.");

                // Map DTO to existing entity
                existingAccount.FirstName = accountDto.FirstName;
                existingAccount.LastName = accountDto.LastName;
                existingAccount.Email = accountDto.Email;
                existingAccount.Password = accountDto.Password;

                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes a specific account by ID.
        /// </summary>
        /// <param name="id">The ID of the account to delete.</param>
        /// <returns>Returns 204 No Content if successful, or 404 Not Found if the account does not exist.</returns>
        /// <response code="204">Account deleted successfully.</response>
        /// <response code="404">Account not found.</response>
        /// <example>DELETE: api/accounts/5</example>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
            {
                return NotFound("Account not found.");
            }

            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Checks if an account exists by ID.
        /// </summary>
        /// <param name="id">The ID of the account.</param>
        /// <returns>Returns true if the account exists, false otherwise.</returns>
        private bool AccountExists(int id)
        {
            return _context.Accounts.Any(e => e.AccountId == id);
        }
    }
}
