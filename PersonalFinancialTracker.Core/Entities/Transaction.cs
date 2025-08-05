using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinancialTracker.Core.Entities
{
    public  class Transaction
    {
        [Key]
        public Guid TransationId { get; set; }
        public string Title { get; set; }

        public string payor { get; set; }
        public string payee { get; set; }

        public decimal Amount { get; set; }
        public string ?Description { get; set; }
        public string TypeOfTransaction { get; set; }
        public DateTime Created { get; set; }


    }
}
