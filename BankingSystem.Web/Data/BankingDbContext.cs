using BankingSystem.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace BankingSystem.Web.Data
{
    // The database context class for the banking system
    public class BankingDbContext : DbContext
    {
        public BankingDbContext(DbContextOptions<BankingDbContext> options) : base(options)
        {
        }

        // Database tables
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ensure AccountNumber is unique
            modelBuilder.Entity<Account>()
                .HasIndex(a => a.AccountNumber)
                .IsUnique();

            // Seed Data for Customers
            modelBuilder.Entity<Customer>().HasData(
                new Customer { CustomerId = 1, FirstName = "Student", LastName = "User", Email = "student@exam.com" },
                new Customer { CustomerId = 2, FirstName = "Bob", LastName = "Smith", Email = "bob.smith@email.com" }
            );

            // Seed Data for Accounts
            modelBuilder.Entity<Account>().HasData(
                new Account { AccountId = 1, AccountNumber = "ACC-10001", Balance = 5000.00m, AccountType = AccountType.Checking, CustomerId = 1 },
                new Account { AccountId = 2, AccountNumber = "ACC-10002", Balance = 12000.00m, AccountType = AccountType.Savings, CustomerId = 1 },
                new Account { AccountId = 3, AccountNumber = "ACC-10003", Balance = 3500.00m, AccountType = AccountType.Checking, CustomerId = 2 },
                new Account { AccountId = 4, AccountNumber = "ACC-10004", Balance = 8000.00m, AccountType = AccountType.Savings, CustomerId = 2 }
            );
        }
    }
}
