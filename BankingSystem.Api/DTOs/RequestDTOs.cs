using System.ComponentModel.DataAnnotations;

namespace BankingSystem.Api.DTOs
{
    // Data Transfer Object for withdrawal requests
    public class WithdrawRequest
    {
        [Required]
        public int AccountId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero")]
        public decimal Amount { get; set; }
    }

    // Data Transfer Object for deposit requests
    public class DepositRequest
    {
        [Required]
        public int AccountId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero")]
        public decimal Amount { get; set; }
    }
}
