using BankManagment_Domain.Entity;

namespace BankManagment_Services
{
    public interface IAccountTypeService
    {
        Task<IEnumerable<AccountType>> GetAllAccountTypesAsync();
        Task CreateAccountTypeAsync(AccountType accountType);
        Task UpdateAccountTypeAsync(Guid id, AccountType updatedAccountType);
        Task DeleteAccountTypeAsync(Guid id);
        Task SaveChangesAsync();
    }
}
