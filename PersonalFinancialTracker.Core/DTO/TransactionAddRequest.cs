using PersonalFinancialTracker.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinancialTracker.Core.DTO
{
    public record TransactionAddRequest(
        string Title, 
        string Payor, 
        string Payee, 
        decimal Amount, 
        string? Description, 
        TransactionType TypeOfTransaction, 
        DateTime Created)
    {
        // Don't include UserId here - it will come from JWT token
        public TransactionAddRequest() : this(default, default, default, default, default, default, default)
        {
        }
    }
}
