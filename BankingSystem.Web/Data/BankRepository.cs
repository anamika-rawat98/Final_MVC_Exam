using BankingSystem.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.Web.Data
{
    // Implementation of the IBankRepository using Entity Framework Core
    public class BankRepository : IBankRepository
    {
        private readonly BankingDbContext _context;

        public BankRepository(BankingDbContext context)
        {
            _context = context;
        }

        // Retrieves all accounts including customer information
        public async Task<IEnumerable<Account>> GetAllAccountsAsync()
        {
            return await _context.Accounts
                .Include(a => a.Customer)
                .ToListAsync();
        }

        // Retrieves a single account by ID including customer and transactions
        public async Task<Account?> GetAccountByIdAsync(int id)
        {
            return await _context.Accounts
                .Include(a => a.Customer)
                .Include(a => a.Transactions)
                .FirstOrDefaultAsync(a => a.AccountId == id);
        }

        // Retrieves an account by its unique account number
        public async Task<Account?> GetAccountByNumberAsync(string accountNumber)
        {
            return await _context.Accounts
                .FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);
        }

        // Adds a new transaction and updates the account balance atomically
        public async Task<bool> AddTransactionAsync(Transaction transaction)
        {
            using var dbTransaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Add the transaction record
                _context.Transactions.Add(transaction);

                // Update the account balance
                var account = await _context.Accounts.FindAsync(transaction.AccountId);
                if (account == null) return false;

                if (transaction.TransactionType == TransactionType.Deposit)
                {
                    account.Balance += transaction.Amount;
                }
                else if (transaction.TransactionType == TransactionType.Withdrawal)
                {
                    if (account.Balance < transaction.Amount) return false; // Safety check
                    account.Balance -= transaction.Amount;
                }

                await _context.SaveChangesAsync();
                await dbTransaction.CommitAsync();
                return true;
            }
            catch
            {
                await dbTransaction.RollbackAsync();
                return false;
            }
        }

        // Retrieves all transactions for a specific account, ordered by date descending
        public async Task<IEnumerable<Transaction>> GetTransactionsByAccountIdAsync(int accountId)
        {
            return await _context.Transactions
                .Where(t => t.AccountId == accountId)
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();
        }

        // Retrieves customer details
        public async Task<Customer?> GetCustomerByIdAsync(int id)
        {
            return await _context.Customers.FindAsync(id);
        }

        // Directly updates the account balance (useful for API operations)
        public async Task<bool> UpdateAccountBalanceAsync(int accountId, decimal newBalance)
        {
            var account = await _context.Accounts.FindAsync(accountId);
            if (account == null) return false;

            account.Balance = newBalance;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
