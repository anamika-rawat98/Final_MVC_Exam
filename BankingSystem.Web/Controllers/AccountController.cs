using BankingSystem.Web.Data;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Web.Controllers
{
    // Controller to handle bank account information
    public class AccountController : Controller
    {
        private readonly IBankRepository _repository;

        public AccountController(IBankRepository repository)
        {
            _repository = repository;
        }

        // GET: Account/Index
        // Displays a list of all bank accounts
        public async Task<IActionResult> Index()
        {
            var accounts = await _repository.GetAllAccountsAsync();
            return View(accounts);
        }

        // GET: Account/Details/5
        // Displays details for a specific account including its balance
        public async Task<IActionResult> Details(int id)
        {
            var account = await _repository.GetAccountByIdAsync(id);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }
    }
}
