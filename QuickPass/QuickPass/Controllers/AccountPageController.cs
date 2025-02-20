using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QuickPass.Interfaces;
using QuickPass.Models;
using QuickPass.Models.ViewModels;

namespace QuickPass.Controllers
{
    public class AccountPageController : Controller
    {
        private readonly IAccountService _accountService;


        public AccountPageController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        [HttpGet]
        public IActionResult New()
        {
            return View();
        }

        // POST: Account/New
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult New(Account account)
        {
            if (ModelState.IsValid)
            {
                // Save the account to the database
                // Redirect to the list page or another appropriate page
                return RedirectToAction("Index");
            }
            return View(account);
        }

        

        //private IEnumerable<Account> GetAccounts()
        //{
        //    // Placeholder for actual data retrieval logic
        //    return new List<Account>
        //    {
              
        //    };
        //}

        // Add this action
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

        // GET: AccountPageController1/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AccountPageController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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

        // GET: AccountPageController1/ConfirmDelete
        public ActionResult ConfirmDelete(int id)
        {
            return View();
        }


        // POST: AccountPageController1/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmDelete(int id, IFormCollection collection)
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
    }
}
