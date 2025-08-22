using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalFinancialTracker.Core.DTO;
using PersonalFinancialTracker.Core.ServiceContracts;
using System.Security.Claims;

namespace PersonalFinancialTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Uncomment this
    public class TransactionController : ControllerBase
    {
        private readonly IFinancialTransactionServices _transactionServices;
        private readonly ILogger<TransactionController> _logger;

        public TransactionController(IFinancialTransactionServices transactionServices, ILogger<TransactionController> logger)
        {
            _transactionServices = transactionServices;
            _logger = logger;
        }

        private Guid GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                           ?? User.FindFirst("sub")?.Value 
                           ?? User.FindFirst("userId")?.Value;
            
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedAccessException("User ID not found in token");
            }
            
            return userId;
        }

        /// <summary>
        /// Get all transactions
        /// </summary>
        /// <returns>List of transactions</returns>
        [HttpGet]
        public async Task<ActionResult<List<TransactionResponse?>>> GetTransactions()
        {
            try
            {
                var userId = GetCurrentUserId(); // Use this line
                var transactions = await _transactionServices.GetTransactionsByUserId(userId); // Change this
                return Ok(transactions);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving transactions");
                return StatusCode(500, "An error occurred while retrieving transactions");
            }
        }

        /// <summary>
        /// Get transaction by ID
        /// </summary>
        /// <param name="id">Transaction ID</param>
        /// <returns>Transaction details</returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<TransactionResponse?>> GetTransaction(Guid id)
        {
            try
            {
                var userId = GetCurrentUserId();
                var transaction = await _transactionServices.GetTransactionByIdAndUserId(id, userId);
                if (transaction == null)
                {
                    return NotFound($"Transaction with ID {id} not found");
                }
                return Ok(transaction);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving transaction with ID {TransactionId}", id);
                return StatusCode(500, "An error occurred while retrieving the transaction");
            }
        }

        /// <summary>
        /// Create a new transaction
        /// </summary>
        /// <param name="request">Transaction details</param>
        /// <returns>Created transaction</returns>
        [HttpPost]
        public async Task<ActionResult<TransactionResponse?>> CreateTransaction([FromBody] TransactionAddRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = GetCurrentUserId(); // Add this
                var createdTransaction = await _transactionServices.AddTransaction(request, userId); // Change this
                
                if (createdTransaction == null)
                {
                    return BadRequest("Failed to create transaction");
                }

                return CreatedAtAction(
                    nameof(GetTransaction),
                    new { id = createdTransaction.TransactionID },
                    createdTransaction
                );
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (FluentValidation.ValidationException ex)
            {
                _logger.LogWarning("Validation failed for transaction creation: {Errors}", string.Join(", ", ex.Errors));
                return BadRequest(ex.Errors.Select(e => e.ErrorMessage));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating transaction");
                return StatusCode(500, "An error occurred while creating the transaction");
            }
        }

        /// <summary>
        /// Update an existing transaction
        /// </summary>
        /// <param name="id">Transaction ID</param>
        /// <param name="request">Updated transaction details</param>
        /// <returns>Updated transaction</returns>
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<TransactionResponse?>> UpdateTransaction(Guid id, [FromBody] TransactionUpdateRequest request)
        {
            try
            {
                if (id != request.TransactionID)
                {
                    return BadRequest("Transaction ID in URL does not match request body");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var userId = GetCurrentUserId();
                var updatedTransaction = await _transactionServices.UpdateTransaction(request, userId);
                if (updatedTransaction == null)
                {
                    return NotFound($"Transaction with ID {id} not found");
                }

                return Ok(updatedTransaction);
            }
            catch (FluentValidation.ValidationException ex)
            {
                _logger.LogWarning("Validation failed for transaction update: {Errors}", string.Join(", ", ex.Errors));
                return BadRequest(ex.Errors.Select(e => e.ErrorMessage));
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Transaction not found for update: {Message}", ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating transaction with ID {TransactionId}", id);
                return StatusCode(500, "An error occurred while updating the transaction");
            }
        }

        /// <summary>
        /// Delete a transaction
        /// </summary>
        /// <param name="id">Transaction ID</param>
        /// <returns>Success status</returns>
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteTransaction(Guid id)
        {
            try
            {
                var userId = GetCurrentUserId();
                var result = await _transactionServices.DeleteTransaction(id, userId);
                if (!result)
                {
                    return NotFound($"Transaction with ID {id} not found");
                }

                return NoContent();
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("Transaction not found for deletion: {Message}", ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting transaction with ID {TransactionId}", id);
                return StatusCode(500, "An error occurred while deleting the transaction");
            }
        }

        /// <summary>
        /// Get transactions by date range
        /// </summary>
        /// <param name="startDate">Start date</param>
        /// <param name="endDate">End date</param>
        /// <returns>Filtered transactions</returns>
        [HttpGet("by-date-range")]
        public async Task<ActionResult<IEnumerable<TransactionResponse?>>> GetTransactionsByDateRange(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            try
            {
                if (startDate > endDate)
                {
                    return BadRequest("Start date cannot be greater than end date");
                }

                var userId = GetCurrentUserId();
                var transactions = await _transactionServices.GetTransactionsByConditionAndUserId(
                    t => t.Created >= startDate && t.Created <= endDate, userId);

                return Ok(transactions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving transactions by date range");
                return StatusCode(500, "An error occurred while retrieving transactions");
            }
        }
    }
}