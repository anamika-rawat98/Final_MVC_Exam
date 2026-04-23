using BankingSystem.Web.Data;
using BankingSystem.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Web.Controllers
{
    // Controller to handle financial transactions (Deposits and Withdrawals)
    public class TransactionController : Controller
    {
        private readonly IBankRepository _repository;

        public TransactionController(IBankRepository repository)
        {
            _repository = repository;
        }

        // GET: Transaction/Deposit
        // Displays the deposit form for a specific account
        public async Task<IActionResult> Deposit(int? accountId)
        {
            if (accountId == null)
            {
                // If no ID is provided, redirect to account list
                return RedirectToAction("Index", "Account");
            }

            var account = await _repository.GetAccountByIdAsync(accountId.Value);
            if (account == null) return NotFound();

            // Store balance for display in view
            ViewBag.CurrentBalance = account.Balance;
            ViewBag.AccountNumber = account.AccountNumber;

            return View(new Transaction { AccountId = accountId.Value, TransactionType = TransactionType.Deposit });
        }

        // POST: Transaction/Deposit
        // Processes the deposit request
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deposit(Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                transaction.TransactionDate = DateTime.Now;
                transaction.TransactionType = TransactionType.Deposit;

                var result = await _repository.AddTransactionAsync(transaction);
                if (result)
                {
                    TempData["Success"] = "Deposit successful!";
                    return RedirectToAction("Details", "Account", new { id = transaction.AccountId });
                }
                ModelState.AddModelError("", "Transaction failed. Please try again.");
            }

            // If we reach here, something failed, redisplay form
            var account = await _repository.GetAccountByIdAsync(transaction.AccountId);
            ViewBag.CurrentBalance = account?.Balance ?? 0;
            ViewBag.AccountNumber = account?.AccountNumber ?? "N/A";
            return View(transaction);
        }

        // GET: Transaction/Withdraw
        // Displays the withdrawal form for a specific account
        public async Task<IActionResult> Withdraw(int? accountId)
        {
            if (accountId == null)
            {
                return RedirectToAction("Index", "Account");
            }

            var account = await _repository.GetAccountByIdAsync(accountId.Value);
            if (account == null) return NotFound();

            ViewBag.CurrentBalance = account.Balance;
            ViewBag.AccountNumber = account.AccountNumber;

            return View(new Transaction { AccountId = accountId.Value, TransactionType = TransactionType.Withdrawal });
        }

        // POST: Transaction/Withdraw
        // Processes the withdrawal request
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Withdraw(Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                var account = await _repository.GetAccountByIdAsync(transaction.AccountId);
                if (account == null) return NotFound();

                if (account.Balance < transaction.Amount)
                {
                    ModelState.AddModelError("Amount", "Insufficient funds.");
                }
                else
                {
                    transaction.TransactionDate = DateTime.Now;
                    transaction.TransactionType = TransactionType.Withdrawal;

                    var result = await _repository.AddTransactionAsync(transaction);
                    if (result)
                    {
                        TempData["Success"] = "Withdrawal successful!";
                        return RedirectToAction("Details", "Account", new { id = transaction.AccountId });
                    }
                    ModelState.AddModelError("", "Transaction failed. Please try again.");
                }
            }

            // Redisplay form with errors
            var currentAccount = await _repository.GetAccountByIdAsync(transaction.AccountId);
            ViewBag.CurrentBalance = currentAccount?.Balance ?? 0;
            ViewBag.AccountNumber = currentAccount?.AccountNumber ?? "N/A";
            return View(transaction);
        }

        // GET: Transaction/History
        // Displays the transaction history for a specific account
        public async Task<IActionResult> History(int? accountId)
        {
            if (accountId == null)
            {
                return RedirectToAction("Index", "Account");
            }

            var account = await _repository.GetAccountByIdAsync(accountId.Value);
            if (account == null) return NotFound();

            var transactions = await _repository.GetTransactionsByAccountIdAsync(accountId.Value);
            
            ViewBag.AccountNumber = account.AccountNumber;
            ViewBag.CurrentBalance = account.Balance;
            ViewBag.AccountId = account.AccountId;

            return View(transactions);
        }
    }
}
