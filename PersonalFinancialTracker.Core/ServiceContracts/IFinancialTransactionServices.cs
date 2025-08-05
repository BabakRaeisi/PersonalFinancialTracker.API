using PersonalFinancialTracker.Core.DTO;
using PersonalFinancialTracker.Core.Entities;
using System.Linq.Expressions;


namespace PersonalFinancialTracker.Core.ServiceContracts
{
    public interface IFinancialTransactionServices
    {
        Task<List<TransactionResponse?>>GetTransactions();

        Task<IEnumerable<TransactionResponse?>> GetTransactionsByConditionAsync(Expression<Func<Transaction,bool>>ConditionExpression );

        Task<TransactionResponse?>GetTransactionByCondition(Expression<Func<Transaction, bool>> ConditionExpression);

        Task<TransactionResponse?> AddTransaction(TransactionAddRequest Transaction);

        Task<TransactionResponse?> UpdateTransaction(TransactionUpdateRequest Transaction);

        Task<bool> DeleteTransaction(Guid TransactionID);


    }
}
