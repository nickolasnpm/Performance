namespace Performance.Application.DTOs.BankTransactions
{
    public record BankTransactionDTO(        
        string Id,
        decimal BaseAmount,
        decimal FeeAmount,
        decimal SettlementAmount,
        int TransactionType,
        int Status,
        string MerchantName,
        string ReferenceNumber);
}