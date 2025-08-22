using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonalFinancialTracker.Core.Enums;

namespace PersonalFinancialTracker.Core.Entities
{
    public class Transaction
    {
        [Key]
        public Guid TransactionId { get; set; }
        
        [Required]
        public Guid UserId { get; set; }  // This links to ApplicationUser.UserID from Auth service
        
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(100)]
        public string payor { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(100)]
        public string payee { get; set; } = string.Empty;
        
        [Required]
        public decimal Amount { get; set; }
        
        [MaxLength(500)]
        public string? Description { get; set; }
        
        [Required]
        public TransactionType TypeOfTransaction { get; set; }
        
        [Required]
        public DateTime Created { get; set; }
    }
}
