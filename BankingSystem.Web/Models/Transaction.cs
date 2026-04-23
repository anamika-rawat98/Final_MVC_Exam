/*
 * Name: Anamika Rawat
 * StudentID: 9087089
 */
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingSystem.Web.Models
{
    // Represents a banking transaction (Deposit or Withdrawal)
    public class Transaction
    {
        [Key] // Primary Key
        public int TransactionId { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Transaction type is required")]
        public TransactionType TransactionType { get; set; }

        // Automatically set the transaction date to now
        public DateTime TransactionDate { get; set; } = DateTime.Now;

        // Foreign Key to Account
        [Required]
        public int AccountId { get; set; }

        // Navigation property to Account
        [ForeignKey("AccountId")]
        public Account? Account { get; set; }

        [MaxLength(200, ErrorMessage = "Description cannot exceed 200 characters")]
        public string? Description { get; set; }
    }
}
