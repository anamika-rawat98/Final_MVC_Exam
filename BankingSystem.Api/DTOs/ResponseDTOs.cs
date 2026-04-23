namespace BankingSystem.Api.DTOs
{
    // Data Transfer Object for account balance responses
    public class BalanceResponse
    {
        public int AccountId { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public decimal Balance { get; set; }
    }

    // Data Transfer Object for transaction history responses
    public class TransactionResponse
    {
        public int TransactionId { get; set; }
        public decimal Amount { get; set; }
        public string TransactionType { get; set; } = string.Empty;
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
