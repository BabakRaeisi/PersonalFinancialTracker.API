using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using PersonalFinancialTracker.Core.DTO;
using PersonalFinancialTracker.Core.Entities;
using PersonalFinancialTracker.Core.RepositoryContracts;
using PersonalFinancialTracker.Core.ServiceContracts;
using System.Linq.Expressions;

namespace PersonalFinancialTracker.Core.Services
{
    public class TransactionServices : IFinancialTransactionServices
    {
        private readonly IRepositoryServices _repositoryService;
        private readonly IValidator<TransactionAddRequest> _transactionAddValidator;
        private readonly IValidator<TransactionUpdateRequest> _transactionUpdateValidator;
        private readonly IMapper _mapper;

        public TransactionServices(IRepositoryServices repositoryServices,
                                   IValidator<TransactionAddRequest> transactionAddValidator,
                                   IValidator<TransactionUpdateRequest> transactionUpdateValidator,
                                   IMapper mapper)
        {
            _repositoryService = repositoryServices;
            _transactionAddValidator = transactionAddValidator;
            _transactionUpdateValidator = transactionUpdateValidator;
            _mapper = mapper;
        }

        // NEW USER-SPECIFIC METHODS
        public async Task<List<TransactionResponse?>> GetTransactionsByUserId(Guid userId)
        {
            var transactions = await _repositoryService.GetTransactionsByConditionAsync(t => t.UserId == userId);
            var transactionResponses = _mapper.Map<IEnumerable<TransactionResponse>>(transactions);
            return transactionResponses.ToList();
        }

        public async Task<TransactionResponse?> AddTransaction(TransactionAddRequest transactionAddRequest, Guid userId)
        {
            if (transactionAddRequest == null)
            {
                throw new ArgumentNullException(nameof(transactionAddRequest), "Transaction request is null.");
            }

            ValidationResult validationResult = await _transactionAddValidator.ValidateAsync(transactionAddRequest);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            Transaction transactionInput = _mapper.Map<Transaction>(transactionAddRequest);
            transactionInput.UserId = userId; // Set the user ID
            transactionInput.TransactionId = Guid.NewGuid(); // Generate new ID

            Transaction? addedTransaction = await _repositoryService.AddTransaction(transactionInput);

            if (addedTransaction == null)
            {
                return null;
            }

            TransactionResponse transactionResponse = _mapper.Map<TransactionResponse>(addedTransaction);
            return transactionResponse;
        }

        public async Task<TransactionResponse?> UpdateTransaction(TransactionUpdateRequest transactionUpdateRequest, Guid userId)
        {
            // Check if transaction exists and belongs to user
            Transaction? existingTransaction = await _repositoryService.GetTransactionByCondition(
                t => t.TransactionId == transactionUpdateRequest.TransactionID && t.UserId == userId);

            if (existingTransaction == null)
                throw new ArgumentException("Transaction not found or does not belong to user.", nameof(transactionUpdateRequest.TransactionID));

            ValidationResult validationResult = await _transactionUpdateValidator.ValidateAsync(transactionUpdateRequest);

            if (!validationResult.IsValid)
            {
                string errorMessage = string.Join(",", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException(errorMessage);
            }

            Transaction transaction = _mapper.Map<Transaction>(transactionUpdateRequest);
            transaction.UserId = userId; // Ensure UserId is set correctly

            Transaction? updatedTransaction = await _repositoryService.UpdateTransaction(transaction);
            TransactionResponse? updatedTransactionResponse = _mapper.Map<TransactionResponse>(updatedTransaction);

            return updatedTransactionResponse;
        }

        public async Task<bool> DeleteTransaction(Guid transactionId, Guid userId)
        {
            // Check if transaction exists and belongs to user
            Transaction? existingTransaction = await _repositoryService.GetTransactionByCondition(
                t => t.TransactionId == transactionId && t.UserId == userId);

            if (existingTransaction == null)
            {
                return false; // Transaction not found or doesn't belong to user
            }

            bool isDeleted = await _repositoryService.DeleteTransaction(transactionId);
            return isDeleted;
        }

        public async Task<TransactionResponse?> GetTransactionByIdAndUserId(Guid transactionId, Guid userId)
        {
            Transaction? transaction = await _repositoryService.GetTransactionByCondition(
                t => t.TransactionId == transactionId && t.UserId == userId);

            if (transaction == null)
            {
                return null;
            }

            TransactionResponse? transactionResponse = _mapper.Map<TransactionResponse>(transaction);
            return transactionResponse;
        }

        public async Task<IEnumerable<TransactionResponse?>> GetTransactionsByConditionAndUserId(Expression<Func<Transaction, bool>> conditionExpression, Guid userId)
        {
            // Simplified approach - get all user transactions first, then filter
            var userTransactions = await _repositoryService.GetTransactionsByConditionAsync(t => t.UserId == userId);
            
            // Apply additional condition in memory (for simplicity)
            var compiledCondition = conditionExpression.Compile();
            var filteredTransactions = userTransactions.Where(t => t != null && compiledCondition(t));
            
            IEnumerable<TransactionResponse?> transactionResponses = _mapper.Map<IEnumerable<TransactionResponse>>(filteredTransactions);
            return transactionResponses;
        }

        // EXISTING METHODS (for backward compatibility - but now they don't filter by user)
        public async Task<TransactionResponse?> AddTransaction(TransactionAddRequest transactionAddRequest)
        {
            throw new NotSupportedException("Use AddTransaction with userId parameter instead.");
        }

        public async Task<bool> DeleteTransaction(Guid TransactionID)
        {
            throw new NotSupportedException("Use DeleteTransaction with userId parameter instead.");
        }

        public async Task<TransactionResponse?> GetTransactionByCondition(Expression<Func<Transaction, bool>> ConditionExpression)
        {
            Transaction? searchedTransaction = await _repositoryService.GetTransactionByCondition(ConditionExpression);
            if (searchedTransaction == null)
            {
                return null;
            }
            TransactionResponse? transactionResponse = _mapper.Map<TransactionResponse>(searchedTransaction);
            return transactionResponse;
        }

        public async Task<List<TransactionResponse?>> GetTransactions()
        {
            throw new NotSupportedException("Use GetTransactionsByUserId instead.");
        }

        public async Task<IEnumerable<TransactionResponse?>> GetTransactionsByConditionAsync(Expression<Func<Transaction, bool>> ConditionExpression)
        {
            IEnumerable<Transaction?> searchedTransactions = await _repositoryService.GetTransactionsByConditionAsync(ConditionExpression);
            IEnumerable<TransactionResponse?> transactionResponses = _mapper.Map<IEnumerable<TransactionResponse>>(searchedTransactions);
            return transactionResponses;
        }

        public async Task<TransactionResponse?> UpdateTransaction(TransactionUpdateRequest transactionUpdateRequest)
        {
            throw new NotSupportedException("Use UpdateTransaction with userId parameter instead.");
        }
    }
}