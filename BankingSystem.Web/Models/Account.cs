/*
 * Name: Anamika Rawat
 * StudentID: 9087089
 */
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingSystem.Web.Models
{
    // Represents a bank account
    public class Account
    {
        [Key] // Primary Key
        public int AccountId { get; set; }

        [Required(ErrorMessage = "Account number is required")]
        [RegularExpression(@"^ACC-\d{5}$", ErrorMessage = "Account number must follow the format ACC-XXXXX")]
        public string AccountNumber { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "Balance cannot be negative")]
        [Column(TypeName = "decimal(18, 2)")] // Specifying precision for SQLite/SQL
        public decimal Balance { get; set; }

        [Required(ErrorMessage = "Account type is required")]
        public AccountType AccountType { get; set; }

        // Foreign Key to Customer
        [Required]
        public int CustomerId { get; set; }

        // Navigation property to Customer
        [ForeignKey("CustomerId")]
        public Customer? Customer { get; set; }

        // Navigation property: One Account has many Transactions
        public ICollection<Transaction>? Transactions { get; set; }
    }
}
