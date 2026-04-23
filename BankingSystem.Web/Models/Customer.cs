/*
 * Name: Anamika Rawat
 * StudentID: 9087089
 */
using System.ComponentModel.DataAnnotations;

namespace BankingSystem.Web.Models
{
    // Represents a bank customer
    public class Customer
    {
        [Key] // Primary Key
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [MaxLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        [MaxLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        // Navigation property: One Customer has many Accounts
        public ICollection<Account>? Accounts { get; set; }
    }
}
