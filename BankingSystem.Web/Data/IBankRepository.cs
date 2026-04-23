/*
 * Name: Anamika Rawat
 * StudentID: 9087089
 */
using BankingSystem.Web.Models;

namespace BankingSystem.Web.Data
{
    // Interface for bank operations to ensure abstraction from DbContext
    public interface IBankRepository
    {
        // Account operations
        Task<IEnumerable<Account>> GetAllAccountsAsync();
        Task<Account?> GetAccountByIdAsync(int id);
        Task<Account?> GetAccountByNumberAsync(string accountNumber);
        
        // Transaction operations
        Task<bool> AddTransactionAsync(Transaction transaction);
        Task<IEnumerable<Transaction>> GetTransactionsByAccountIdAsync(int accountId);
        
        // Customer operations
        Task<Customer?> GetCustomerByIdAsync(int id);

        // Update balance
        Task<bool> UpdateAccountBalanceAsync(int accountId, decimal newBalance);
    }
}
