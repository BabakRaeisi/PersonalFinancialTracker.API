using PersonalFinancialTracker.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinancialTracker.Core.DTO
{
    public record TransactionResponse(
        Guid TransactionID, 
        Guid UserId,  // Add this
        string Title, 
        string Payor, 
        string Payee, 
        decimal Amount, 
        string? Description, 
        TransactionType TypeOfTransaction, 
        DateTime Created)
    {
        public TransactionResponse() : this(default, default, default, default, default, default, default, default, default)
        {
        }
    }

}
