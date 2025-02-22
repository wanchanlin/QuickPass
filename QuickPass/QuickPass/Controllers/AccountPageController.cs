using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuickPass.Interfaces;
using QuickPass.Models;
using QuickPass.Models.ViewModels;
using System.Diagnostics;

namespace QuickPass.Controllers
{
    public class AccountPageController : Controller
    {
        private readonly IAccountService _accountService;


        public AccountPageController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<IActionResult> List()
        {
            var accounts = await _accountService.GetAccounts();
            return View(accounts);
        }
        // GET: AccountPageController1
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult New()
        {
            return View();
        }

        //// POST: Account/New
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult New(Account account)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Save the account to the database
        //        // Redirect to the list page or another appropriate page
        //        return RedirectToAction("Index");
        //    }
        //    return View(account);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        // POST CategoryPage/Add
        [HttpPost]
        public async Task<IActionResult> Add(Account account)
        {
            ServiceResponse response = await _accountService.CreateAccount(account);

            if (response.Status == ServiceResponse.ServiceStatus.Created)
            {
                return RedirectToAction("Details", "AccountPage", new { id = response.CreatedId });
            }
            else
            {
                return View("Error");
            }
        }


      
       
        // GET: AccountPageController1/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}
        public async Task<IActionResult> Details(int id)
        {
            var account = await _accountService.GetAccount(id);
            var tickets = await _accountService.GetTicketsForAccount(id);

            if (account == null)
            {
                return NotFound();
            }

            var accountDTO = new AccountDTO
            {
                AccountId = account.AccountId,
                FirstName = account.FirstName,
                LastName = account.LastName,
                Email = account.Email,
                // Map other properties as needed
            };

            return View(new AccountDetails
            {
                Account = accountDTO,
                AccountTickets = tickets.ToList()
            });
        }

        // GET: AccountPageController1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AccountPageController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        [Authorize]
        // GET: AccountPageController1/Edit/id
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Use the service instead of direct context access
            var account = await _accountService.GetAccount(id.Value);

            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }


        // POST: AccountPageController1/Edit/id

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AccountId,FirstName,LastName,Email,Password")] Account account)
        {
            if (id != account.AccountId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Use the service to update
                    var response = await _accountService.UpdateAccount(account);

                    if (response.Status == ServiceResponse.ServiceStatus.Updated)
                    {
                        return RedirectToAction(nameof(List));
                    }
                    else
                    {
                        return View("Error");
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await AccountExists(account.AccountId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(account);
        }

        private async Task<bool> AccountExists(int id)
        {
            // Implement this check through your service
            var account = await _accountService.GetAccount(id);
            return account != null;
        }
        //// GET: AccountPageController1/ConfirmDelete


        public async Task<IActionResult> ConfirmDelete(int id)
        {
            var account = await _accountService.GetAccount(id); 
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int accountId) // Match parameter name
        {
            var response = await _accountService.DeleteAccount(accountId);
            if (response.Status == ServiceResponse.ServiceStatus.Deleted)
            {
                return RedirectToAction("List");
            }
            return View("Error", new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                //ErrorMessage = response.Messages.FirstOrDefault()
            });
        }


    }
}
