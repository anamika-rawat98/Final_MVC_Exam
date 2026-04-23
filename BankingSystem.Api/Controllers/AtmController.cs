/*
 * Name: Anamika Rawat
 * StudentID: 9087089
 */
using BankingSystem.Api.DTOs;
using BankingSystem.Web.Data;
using BankingSystem.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.Api.Controllers
{
    // API Controller for ATM operations
    [ApiController]
    [Route("api/atm")]
    public class AtmController : ControllerBase
    {
        private readonly IBankRepository _repository;

        public AtmController(IBankRepository repository)
        {
            _repository = repository;
        }

        // GET: /api/atm/balance/{accountId}
        // Returns the current balance for an account
        [HttpGet("balance/{accountId}")]
        public async Task<IActionResult> GetBalance(int accountId)
        {
            var account = await _repository.GetAccountByIdAsync(accountId);
            if (account == null)
            {
                return NotFound(new { error = "Account not found" });
            }

            var response = new BalanceResponse
            {
                AccountId = account.AccountId,
                AccountNumber = account.AccountNumber,
                Balance = account.Balance
            };

            return Ok(response);
        }

        // POST: /api/atm/withdraw
        // Processes a withdrawal from an account
        [HttpPost("withdraw")]
        public async Task<IActionResult> Withdraw([FromBody] WithdrawRequest request)
        {
            // Explicitly check for invalid amount to return exact specified JSON format
            if (request.Amount <= 0)
            {
                return BadRequest(new { error = "Amount must be greater than zero" });
            }

            var account = await _repository.GetAccountByIdAsync(request.AccountId);
            if (account == null)
            {
                return NotFound(new { error = "Account not found" });
            }

            if (account.Balance < request.Amount)
            {
                return BadRequest(new { error = "Insufficient funds", currentBalance = account.Balance });
            }

            var transaction = new Transaction
            {
                AccountId = request.AccountId,
                Amount = request.Amount,
                TransactionType = TransactionType.Withdrawal,
                TransactionDate = DateTime.Now,
                Description = "ATM Withdrawal"
            };

            var result = await _repository.AddTransactionAsync(transaction);
            if (result)
            {
                // Re-fetch to get updated balance
                var updatedAccount = await _repository.GetAccountByIdAsync(request.AccountId);
                return Ok(new { message = "Withdrawal successful", newBalance = updatedAccount?.Balance });
            }

            return BadRequest(new { error = "Withdrawal failed" });
        }

        // POST: /api/atm/deposit
        // Processes a deposit into an account
        [HttpPost("deposit")]
        public async Task<IActionResult> Deposit([FromBody] DepositRequest request)
        {
            // Explicitly check for invalid amount to return exact specified JSON format
            if (request.Amount <= 0)
            {
                return BadRequest(new { error = "Amount must be greater than zero" });
            }
            var account = await _repository.GetAccountByIdAsync(request.AccountId);
            if (account == null)
            {
                return NotFound(new { error = "Account not found" });
            }

            var transaction = new Transaction
            {
                AccountId = request.AccountId,
                Amount = request.Amount,
                TransactionType = TransactionType.Deposit,
                TransactionDate = DateTime.Now,
                Description = "ATM Deposit"
            };

            var result = await _repository.AddTransactionAsync(transaction);
            if (result)
            {
                var updatedAccount = await _repository.GetAccountByIdAsync(request.AccountId);
                return Ok(new { message = "Deposit successful", newBalance = updatedAccount?.Balance });
            }

            return BadRequest(new { error = "Deposit failed" });
        }

        // GET: /api/atm/transactions/{accountId}
        // Returns the transaction history for an account
        [HttpGet("transactions/{accountId}")]
        public async Task<IActionResult> GetTransactions(int accountId)
        {
            var account = await _repository.GetAccountByIdAsync(accountId);
            if (account == null)
            {
                return NotFound(new { error = "Account not found" });
            }

            var transactions = await _repository.GetTransactionsByAccountIdAsync(accountId);
            
            var response = transactions.Select(t => new TransactionResponse
            {
                TransactionId = t.TransactionId,
                Amount = t.Amount,
                TransactionType = t.TransactionType.ToString(),
                TransactionDate = t.TransactionDate,
                Description = t.Description ?? string.Empty
            }).ToList();

            return Ok(response);
        }
    }
}
