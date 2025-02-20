using QuickPass.Controllers;
using QuickPass.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuickPass.Interfaces
{
    public interface IAccountService
    {
        // Base CRUD
        Task<IEnumerable<Account>> GetAccounts();
        Task<Account?> GetAccount(int id);
        Task<ServiceResponse> CreateAccount(Account account);
        Task<ServiceResponse> UpdateAccount(Account account);
        Task<ServiceResponse> DeleteAccount(int id);

        // Return TicketDTO instead of Ticket
        Task<IEnumerable<TicketDTO>> GetTicketsForAccount(int accountId);
    }
}