using QuickPass.Interfaces;
using QuickPass.Models.ViewModels;
using QuickPass.Models;
using QuickPass.Data;
using Microsoft.EntityFrameworkCore;

namespace QuickPass.Services
{
    // The AccountService class provides various methods for managing user accounts.
    public class AccountService : IAccountService
    {
        // The ApplicationDbContext provides the database context for accessing the database.
        private readonly ApplicationDbContext _context;

        // Constructor injects the ApplicationDbContext to interact with the database.
        public AccountService(ApplicationDbContext context)
        {
            _context = context;
        }

        // This method retrieves all accounts from the database asynchronously.
        public async Task<IEnumerable<Account>> GetAccounts()
        {
            return await _context.Accounts.ToListAsync();
        }

        // This method retrieves a single account by its ID asynchronously.
        // It uses a try-catch block to handle potential exceptions when fetching the account.
        public async Task<Account?> GetAccount(int id)
        {
            try
            {
                return await _context.Accounts.FindAsync(id);
            }
            catch (Exception ex)
            {
                // Log the exception (to be implemented in a real application)
                throw new Exception("Error fetching account: " + ex.Message);
            }
        }

        // This method is responsible for creating a new account asynchronously.
        // It adds the account to the database and saves changes.
        public async Task<ServiceResponse> CreateAccount(Account account)
        {
            var response = new ServiceResponse();

            try
            {
                // Add the account to the database.
                _context.Accounts.Add(account);
                await _context.SaveChangesAsync();

                // Set the response to indicate the account was created successfully.
                response.Status = ServiceResponse.ServiceStatus.Created;
                response.CreatedId = account.AccountId;
            }
            catch (DbUpdateException ex)
            {
                // Handle any exceptions related to the database update.
                response.Status = ServiceResponse.ServiceStatus.Error;
                response.Messages.Add("Error creating account: " + ex.Message);
            }
            return response;
        }

        // This method updates an existing account asynchronously.
        // It modifies the state of the account and saves changes to the database.
        public async Task<ServiceResponse> UpdateAccount(Account account)
        {
            var response = new ServiceResponse();

            try
            {
                // Mark the account entity as modified.
                _context.Entry(account).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                // Set the response to indicate the account was updated successfully.
                response.Status = ServiceResponse.ServiceStatus.Updated;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Handle concurrency errors if the account no longer exists.
                if (!await AccountExists(account.AccountId))
                {
                    // Account not found; update the response accordingly.
                    response.Messages.Add("Account not found");
                }
                else
                {
                    // Handle other database update exceptions.
                    response.Status = ServiceResponse.ServiceStatus.Error;
                    response.Messages.Add("Error updating account: " + ex.Message);
                }
            }
            return response;
        }

        // This method deletes an account by its ID asynchronously.
        // It checks if the account exists, then attempts to remove it from the database.
        public async Task<ServiceResponse> DeleteAccount(int id)
        {
            var response = new ServiceResponse();
            var account = await _context.Accounts.FindAsync(id);

            // If the account does not exist, return an error response.
            if (account == null)
            {
                response.Messages.Add("Account not found");
                return response;
            }

            try
            {
                // Remove the account from the database.
                _context.Accounts.Remove(account);
                await _context.SaveChangesAsync();

                // Set the response to indicate the account was deleted successfully.
                response.Status = ServiceResponse.ServiceStatus.Deleted;
            }
            catch (DbUpdateException ex)
            {
                // Handle any exceptions related to the database update.
                response.Status = ServiceResponse.ServiceStatus.Error;
                response.Messages.Add("Error deleting account: " + ex.Message);
            }
            return response;
        }

        // This method retrieves all tickets associated with a specific account asynchronously.
        // It fetches the tickets based on the account ID and returns them as a list of TicketDTOs.
        public async Task<IEnumerable<TicketDTO>> GetTicketsForAccount(int accountId)
        {
            var tickets = await _context.Tickets
                .Where(t => t.AccountId == accountId) // Filter tickets by account ID
                .ToListAsync();

            // Return the tickets as a list of TicketDTOs.
            return tickets.Select(t => new TicketDTO
            {
                TicketId = t.TicketId,
                Price = t.Price,
                SeatNumber = t.SeatNumber,
                BookingDate = t.BookingDate,
                EventId = t.EventId
            }).ToList();
        }

        // This private helper method checks if an account exists based on the provided ID.
        // It returns a boolean indicating whether the account is found in the database.
        private async Task<bool> AccountExists(int id)
        {
            return await _context.Accounts.AnyAsync(e => e.AccountId == id);
        }
    }
}
