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
        private readonly IRepositoryServices _repositoryService ;
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

        public async Task <TransactionResponse?> AddTransaction(TransactionAddRequest transactionAddRequest)
        {
            if (transactionAddRequest == null)
            {
                throw new ArgumentNullException(nameof(transactionAddRequest), "Transaction request is null.");
            }
            ValidationResult validationResult = await _transactionAddValidator.ValidateAsync(transactionAddRequest);
            if (!validationResult.IsValid)
            {
                string.Join(",", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException(validationResult.Errors);
            }

            Transaction transactionInput = _mapper.Map<Transaction>(transactionAddRequest);
            Transaction? addedTransaction = await _repositoryService.AddTransaction(transactionInput);

            if (addedTransaction == null)
            {
                return null;
            }
            TransactionResponse transactionResponse = _mapper.Map<TransactionResponse>(addedTransaction);
            return transactionResponse;
        }

        public  async Task<bool> DeleteTransaction(Guid TransactionID)
        {
           Transaction? existingTransaction =  _repositoryService.GetTransactionByCondition(t => t.TransationId == TransactionID).Result;
            if (existingTransaction == null)
            {
                throw new ArgumentException("Transaction not found.", nameof(TransactionID));
            }
            bool isDeleted =   await  _repositoryService.DeleteTransaction(TransactionID);

            return isDeleted;
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
            IEnumerable<Transaction?> transactions = await _repositoryService.GetTransactions();

            IEnumerable<TransactionResponse?> transactionResponses =  _mapper.Map<IEnumerable<TransactionResponse>>(transactions);
        
            return transactionResponses.ToList();
        }

        public async Task<IEnumerable<TransactionResponse?>> GetTransactionsByConditionAsync(Expression<Func<Transaction, bool>> ConditionExpression)
        {
           IEnumerable<Transaction?> searchedTransactions = await _repositoryService.GetTransactionsByConditionAsync(ConditionExpression);
            
            IEnumerable<TransactionResponse?> transactionResponses = _mapper.Map<IEnumerable<TransactionResponse>>(searchedTransactions);
            return transactionResponses.ToList();
        }

        public async Task<TransactionResponse?> UpdateTransaction(TransactionUpdateRequest transactionUpdateRequest)
        {
            Transaction? existingTransaction =  await _repositoryService.GetTransactionByCondition(t => t.TransationId == transactionUpdateRequest.TransactionID);
            
            if (existingTransaction == null)
                throw new ArgumentException ("Transaction not found.", nameof(transactionUpdateRequest.TransactionID));

            ValidationResult validationResult = await _transactionUpdateValidator.ValidateAsync(transactionUpdateRequest);

            if (!validationResult.IsValid)
            {
                string errorMessage = string.Join(",", validationResult.Errors.Select(e => e.ErrorMessage));
                throw new ValidationException(errorMessage); 
            }
            
            Transaction transaction = _mapper.Map<Transaction>(transactionUpdateRequest);
            Transaction? updatedTransaction = await _repositoryService.UpdateTransaction(transaction);

            TransactionResponse? UpdatedtransactionResponse = _mapper.Map<TransactionResponse>(updatedTransaction);


            return UpdatedtransactionResponse;
        }
    }
}
