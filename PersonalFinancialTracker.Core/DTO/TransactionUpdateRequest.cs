using PersonalFinancialTracker.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinancialTracker.Core.DTO
{
    public record TransactionUpdateRequest(Guid TransactionID, string Title, string Payor, string Payee, decimal Amount, string? Description, TransactionType TypeOfTransaction, DateTime Created)
    {
        public TransactionUpdateRequest() : this(default, default, default, default, default, default, default, default)
        {

        }
    }
}
