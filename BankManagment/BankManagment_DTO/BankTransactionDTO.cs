using BankManagment_Domain.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankManagment_DTO
{
    public class BankTransactionDTO
    {
        public Guid Id { get; set; }
        public string TransactionPersonFirstName { get; set; }
        public string? TransactionPersonMiddleName { get; set; }
        [Required]
        public string TransactionPersonLastName { get; set; }
        public string? TransactionType { get; set; }
        public string? Category { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public Guid PaymentMethodID { get; set; }
        public Guid BankAccountID { get; set; }

        public virtual PaymentMethod PaymentMethod { get; set; }
        public virtual BankAccount BankAccount { get; set; }
    }
}
