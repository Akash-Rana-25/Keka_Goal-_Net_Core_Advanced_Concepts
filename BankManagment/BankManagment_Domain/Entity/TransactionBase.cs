using BankManagment_Domain.Entity;
using System.ComponentModel.DataAnnotations;

public abstract class TransactionBase
{
    public Guid Id { get; set; }
    [Required]
    public string? TransactionPersonFirstName { get; set; }
    public string? TransactionPersonMiddleName { get; set; }
    [Required]
    public string? TransactionPersonLastName { get; set; }
    public string? TransactionType { get; set; }
    public string? Category { get; set; }
    public decimal Amount { get; set; }
    public DateTime TransactionDate { get; set; }
    public Guid PaymentMethodID { get; set; }
    public Guid BankAccountID { get; set; }

    public virtual PaymentMethod PaymentMethod { get; set; }
    public virtual BankAccount BankAccount { get; set; }
}
