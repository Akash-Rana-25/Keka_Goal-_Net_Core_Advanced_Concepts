using BankManagment_Domain.Entity;


namespace BankManagment_Services
{
    public interface IBankAccountService
    {
        Task<IEnumerable<BankAccount>> GetAllBankAccountsAsync();
        Task CreateBankAccountAsync(BankAccount bankAccount);
        Task UpdateBankAccountAsync(Guid id, BankAccount updatedBankAccount);
        Task DeleteBankAccountAsync(Guid id);
        Task SaveChangesAsync();
    }

}
