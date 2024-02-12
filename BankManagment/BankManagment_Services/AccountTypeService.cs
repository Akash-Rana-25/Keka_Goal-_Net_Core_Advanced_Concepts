using BankManagment_Domain.Entity;
using BankManagment_Infrastructure.Repository;
using BankManagment_Infrastructure.UnitOfWork;
using Serilog;

namespace BankManagment_Services
{
    public class AccountTypeService : IAccountTypeService
    {
        private readonly IRepository<AccountType> _accountTypeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        public AccountTypeService(IUnitOfWork unitOfWork, IRepository<AccountType> accountTypeRepository)
        {
            _unitOfWork = unitOfWork;
            _accountTypeRepository = accountTypeRepository;
            _logger = Log.ForContext<AccountTypeService>();

        }

        public Task<IEnumerable<AccountType>> GetAllAccountTypesAsync()
        {
            _logger.Information("Getting all account types.");
            return _accountTypeRepository.GetAllAsync();
        }

        public async Task CreateAccountTypeAsync(AccountType accountType)
        {
            _logger.Information("Creating New Account Type");
            await _accountTypeRepository.AddAsync(accountType);
        }

        public async Task UpdateAccountTypeAsync(Guid id, AccountType updatedAccountType)
        {
            _logger.Information($"Updating account type with ID {id}.");
            var existingAccountType = await _accountTypeRepository.GetByIdAsync(id);
            if (existingAccountType == null)
            {
                _logger.Error("Account type not found for updating.");
                throw new ArgumentException("Account type not found.");
            }
            existingAccountType.Name = updatedAccountType.Name;

            await _accountTypeRepository.UpdateAsync(existingAccountType);
        }

        public async Task DeleteAccountTypeAsync(Guid id)
        {
            var accountType = await _accountTypeRepository.GetByIdAsync(id);
            if (accountType == null)
            {
                _logger.Error("Account type not found for Deleting.");
                throw new ArgumentException("Account type not found.");
            }

            await _accountTypeRepository.DeleteAsync(accountType);
        }

        public async Task SaveChangesAsync()
        {
            _logger.Information("Saving Changes");
            await _unitOfWork.SaveAsync();
        }
    }
}
