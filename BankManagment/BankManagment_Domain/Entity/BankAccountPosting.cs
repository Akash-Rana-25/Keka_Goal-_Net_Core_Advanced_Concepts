using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankManagment_Domain.Entity
{
    public class BankAccountPosting : TransactionBase
    {
        public Guid TransactionId { get; set; }
        public virtual BankTransaction Transaction { get; set; }
    }

}
