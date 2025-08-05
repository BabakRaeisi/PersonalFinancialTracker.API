using PersonalFinancialTracker.Core.Entities;
using PersonalFinancialTracker.Core.RepositoryContracts;
using PersonalFinancialTracker.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinancialTracker.Infrastructure.RepositoryServices
{
    internal class TransactionRepository : IRepositoryServices
    {
        private readonly AppDbContext _context;

        public TransactionRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Transaction?> AddTransaction(Transaction Transaction)
        {
           _context.Transactions.Add(Transaction);
            await _context.SaveChangesAsync();
            return Transaction;
        }

        public Task<bool> DeleteTransaction(Guid TransactionID)
        {
            throw new NotImplementedException();
        }

        public Task<Transaction?> GetTransactionByCondition(Expression<Func<Transaction, bool>> ConditionExpression)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Transaction>> GetTransactions()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Transaction?>> GetTransactionsByConditionAsync(Expression<Func<Transaction, bool>> ConditionExpression)
        {
            throw new NotImplementedException();
        }

        public Task<Transaction?> UpdateTransaction(Transaction transaction)
        {
            throw new NotImplementedException();
        }
    }
}
