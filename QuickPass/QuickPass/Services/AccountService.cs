using QuickPass.Interfaces;
using QuickPass.Models;
using QuickPass.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using QuickPass.Controllers;

namespace QuickPass.Services
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext _context;

        public AccountService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Account>> GetAccounts()
        {
            return await _context.Accounts.ToListAsync();
        }

        public async Task<Account?> GetAccount(int id)
        {
            return await _context.Accounts.FindAsync(id);
        }

        public async Task<ServiceResponse> CreateAccount(Account account)
        {
            var response = new ServiceResponse();

            try
            {
                _context.Accounts.Add(account);
                await _context.SaveChangesAsync();
                response.Status = ServiceResponse.ServiceStatus.Created;
                response.CreatedId = account.AccountId;
            }
            catch (DbUpdateException ex)
            {
                response.Status = ServiceResponse.ServiceStatus.Error;
                response.Messages.Add("Error creating account: " + ex.Message);
            }
            return response;
        }

        public async Task<ServiceResponse> UpdateAccount(Account account)
        {
            var response = new ServiceResponse();

            try
            {
                _context.Entry(account).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                response.Status = ServiceResponse.ServiceStatus.Updated;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!await AccountExists(account.AccountId))
                {
                    response.Status = ServiceResponse.ServiceStatus.NotFound;
                    response.Messages.Add("Account not found");
                }
                else
                {
                    response.Status = ServiceResponse.ServiceStatus.Error;
                    response.Messages.Add("Error updating account: " + ex.Message);
                }
            }
            return response;
        }

        public async Task<ServiceResponse> DeleteAccount(int id)
        {
            var response = new ServiceResponse();
            var account = await _context.Accounts.FindAsync(id);

            if (account == null)
            {
                response.Status = ServiceResponse.ServiceStatus.NotFound;
                response.Messages.Add("Account not found");
                return response;
            }

            try
            {
                _context.Accounts.Remove(account);
                await _context.SaveChangesAsync();
                response.Status = ServiceResponse.ServiceStatus.Deleted;
            }
            catch (DbUpdateException ex)
            {
                response.Status = ServiceResponse.ServiceStatus.Error;
                response.Messages.Add("Error deleting account: " + ex.Message);
            }
            return response;
        }

        //public async Task<IEnumerable<Ticket>> GetTicketsForAccount(int accountId)
        //{
        //    return await _context.Tickets
        //        .Where(t => t.AccountId == accountId)
        //        .ToListAsync();
        //}

        //private async Task<bool> AccountExists(int id)
        //{
        //    return await _context.Accounts.AnyAsync(e => e.AccountId == id);
        //}
        public async Task<IEnumerable<TicketDTO>> GetTicketsForAccount(int accountId)
        {
            var tickets = await _context.Tickets
                .Where(t => t.AccountId == accountId)
                .ToListAsync();

            return tickets.Select(t => new TicketDTO
            {
                TicketId = t.TicketId,
                Price = t.Price,
                SeatNumber = t.SeatNumber,
                BookingDate = t.BookingDate,
                EventId = t.EventId
            }).ToList();
        }
        private async Task<bool> AccountExists(int id)
        {
            return await _context.Accounts.AnyAsync(e => e.AccountId == id);
        }
    }
}