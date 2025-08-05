using FluentValidation;
using PersonalFinancialTracker.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinancialTracker.Core.Validators
{
    public class TransactionUpdateRequestValidator : AbstractValidator<TransactionUpdateRequest>
    {
        public TransactionUpdateRequestValidator()
        {
            RuleFor(x => x.TransactionID)
                .NotEmpty().WithMessage("Transaction ID is required.");
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");
            RuleFor(x => x.Payor)
                .NotEmpty().WithMessage("Payor is required.")
                .MaximumLength(100).WithMessage("Payor cannot exceed 100 characters.");
            RuleFor(x => x.Payee)
                .NotEmpty().WithMessage("Payee is required.")
                .MaximumLength(100).WithMessage("Payee cannot exceed 100 characters.");
            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Amount must be greater than zero.");
            RuleFor(x => x.TypeOfTransaction)
                .IsInEnum().WithMessage("Type of transaction must be a valid enum value.");
            RuleFor(x => x.Created)
                .GreaterThan(DateTime.MinValue).WithMessage("Created date must be a valid date.");
        }
    }
}
