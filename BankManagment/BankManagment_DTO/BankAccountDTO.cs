using BankManagment_Domain.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankManagment_DTO
{
    public class BankAccountDTO
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]

        public string AccountNumber { get; set; }
        [Required]
        public DateTime OpeningDate { get; set; }
        public DateTime? ClosingDate { get; set; }

        public Guid AccountTypeId { get; set; }
        public decimal TotalBalance { get; set; }

        public virtual AccountType AccountType { get; set; }
    }
}
