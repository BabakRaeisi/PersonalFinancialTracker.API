using Microsoft.EntityFrameworkCore;
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

        public async Task<bool> DeleteTransaction(Guid TransactionID)
        {
           Transaction? existingTransaction = await _context.Transactions.FirstOrDefaultAsync(t => t.TransactionId == TransactionID);
            if (existingTransaction == null) return false;
            _context.Transactions.Remove(existingTransaction);
            int affectedRows = await _context.SaveChangesAsync();
             
             return affectedRows > 0;
        }

        public async Task<Transaction?> GetTransactionByCondition(Expression<Func<Transaction, bool>> ConditionExpression)
        { 
            return await _context.Transactions.FirstOrDefaultAsync(ConditionExpression);
        }

        public async Task<IEnumerable<Transaction>> GetTransactions()
        {
           return await _context.Transactions.ToListAsync();
        }

        public async Task<IEnumerable<Transaction?>> GetTransactionsByConditionAsync(Expression<Func<Transaction, bool>> ConditionExpression)
        {
          return await _context.Transactions.Where(ConditionExpression).ToListAsync();
        }

        public async Task<Transaction?> UpdateTransaction(Transaction transaction)
        {
           Transaction? existigTransaction = await _context.Transactions.FirstOrDefaultAsync(t => t.TransactionId == transaction.TransactionId);
            if (existigTransaction == null) return null;

            existigTransaction.Title = transaction.Title;
            existigTransaction.payor = transaction.payor;
            existigTransaction.payee = transaction.payee;
            existigTransaction.Amount = transaction.Amount;
            existigTransaction.Description = transaction.Description;
            existigTransaction.TypeOfTransaction = transaction.TypeOfTransaction;
            existigTransaction.Created = transaction.Created;
             
            await _context.SaveChangesAsync();
            return existigTransaction; 
        }
    }
}
