using BankManagment_Domain.Entity;


namespace BankManagment_Services
{
    public interface IBankTransactionService
    {
        Task<IEnumerable<BankTransaction>> GetAllBankTransactionsAsync();
        Task CreateBankTransactionAsync(BankTransaction bankTransaction);
        Task UpdateBankTransactionAsync(Guid id, BankTransaction updatedBankTransaction);
        Task DeleteBankTransactionAsync(Guid id);
        Task SaveChangesAsync();
    }
}
