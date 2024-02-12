using BankManagment_Domain.Entity;
using BankManagment_Infrastructure.Repository;

namespace BankManagment_Infrastructure.UnitOfWork
{
    public interface IUnitOfWork
    {
        IRepository<AccountType> AccountsTypeRepository { get; }

        IRepository<BankAccount> BankAccountsRepository { get; }

        IRepository<BankTransaction> BankTransactionRepository { get; }

        IRepository<PaymentMethod> PaymentMethodRepository { get; }


        Task SaveAsync();
    }
}
