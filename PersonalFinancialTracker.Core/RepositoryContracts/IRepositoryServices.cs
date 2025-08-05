using PersonalFinancialTracker.Core.DTO;
using PersonalFinancialTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinancialTracker.Core.RepositoryContracts
{
    public interface IRepositoryServices
    {
        Task<IEnumerable<Transaction>> GetTransactions();

        Task<IEnumerable<Transaction?>> GetTransactionsByConditionAsync(Expression<Func<Transaction, bool>> ConditionExpression);

        Task<Transaction?> GetTransactionByCondition(Expression<Func<Transaction, bool>> ConditionExpression);

        Task<Transaction?> AddTransaction(Transaction Transaction);

        Task<Transaction?> UpdateTransaction( Transaction transaction);

        Task<bool> DeleteTransaction(Guid TransactionID);
    }
}
