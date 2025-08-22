using PersonalFinancialTracker.Core.DTO;
using PersonalFinancialTracker.Core.Entities;
using System.Linq.Expressions;

namespace PersonalFinancialTracker.Core.ServiceContracts
{
    public interface IFinancialTransactionServices  
    {
        // User-specific methods (RECOMMENDED)
        Task<List<TransactionResponse?>> GetTransactionsByUserId(Guid userId);
        Task<TransactionResponse?> AddTransaction(TransactionAddRequest request, Guid userId);
        Task<TransactionResponse?> UpdateTransaction(TransactionUpdateRequest request, Guid userId);
        Task<bool> DeleteTransaction(Guid transactionId, Guid userId);
        Task<TransactionResponse?> GetTransactionByIdAndUserId(Guid transactionId, Guid userId);
        Task<IEnumerable<TransactionResponse?>> GetTransactionsByConditionAndUserId(Expression<Func<Transaction, bool>> conditionExpression, Guid userId);

        // Keep existing methods for backward compatibility (but they should filter by user internally)
        Task<List<TransactionResponse?>> GetTransactions();
        Task<IEnumerable<TransactionResponse?>> GetTransactionsByConditionAsync(Expression<Func<Transaction, bool>> conditionExpression);
        Task<TransactionResponse?> GetTransactionByCondition(Expression<Func<Transaction, bool>> conditionExpression);
        Task<TransactionResponse?> AddTransaction(TransactionAddRequest transaction);
        Task<TransactionResponse?> UpdateTransaction(TransactionUpdateRequest transaction);
        Task<bool> DeleteTransaction(Guid transactionID);
    }
}
