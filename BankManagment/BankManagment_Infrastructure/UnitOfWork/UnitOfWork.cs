using BankManagment_Domain.Entity;
using BankManagment_Infrastructure.Context;
using BankManagment_Infrastructure.Repository;


namespace BankManagment_Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            BankAccountsRepository = new Repository<BankAccount>(context);
            AccountsTypeRepository = new Repository<AccountType>(context);
            BankTransactionRepository = new Repository<BankTransaction>(context);
            PaymentMethodRepository = new Repository<PaymentMethod>(context);


        }

        public IRepository<BankAccount> BankAccountsRepository { get; private set; }
        public IRepository<AccountType> AccountsTypeRepository { get; private set; }
        public IRepository<BankTransaction> BankTransactionRepository { get; private set; }
        public IRepository<PaymentMethod> PaymentMethodRepository { get; private set; }



        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
