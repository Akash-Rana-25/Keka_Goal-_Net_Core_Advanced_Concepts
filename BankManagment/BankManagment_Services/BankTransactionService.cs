using BankManagment_Domain.Entity;
using BankManagment_Infrastructure.Repository;
using BankManagment_Infrastructure.UnitOfWork;
using Serilog;

namespace BankManagment_Services
{
    public class BankTransactionService : IBankTransactionService
    {
        private readonly IRepository<BankTransaction> _bankTransactionRepository;
        private readonly IRepository<BankAccountPosting> _bankAccountPostingRepository;
        private readonly IRepository<BankAccount> _bankAccountRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;


        public BankTransactionService(IUnitOfWork unitOfWork,
                                      IRepository<BankTransaction> bankTransactionRepository,
                                      IRepository<BankAccountPosting> bankAccountPostingRepository,
                                      IRepository<BankAccount> bankAccountRepository)
        {
            _unitOfWork = unitOfWork;
            _bankTransactionRepository = bankTransactionRepository;
            _bankAccountPostingRepository = bankAccountPostingRepository;
            _bankAccountRepository = bankAccountRepository;
            _logger=Log.ForContext<BankTransactionService>();
        }

        public  Task<IEnumerable<BankTransaction>> GetAllBankTransactionsAsync()
        {
            _logger.Information("Geting All Bank Transaction");
            return _bankTransactionRepository.GetAllAsync();
        }

        public async Task CreateBankTransactionAsync(BankTransaction bankTransaction)
        {
           
            if (_bankTransactionRepository == null || _bankAccountPostingRepository == null || _bankAccountRepository == null)
            {
                _logger.Error("Repositories not properly initialized.");
                throw new InvalidOperationException("Repositories not properly initialized.");
            }
            _logger.Information("Transaction Created");
            await _bankTransactionRepository.AddAsync(bankTransaction);

            if (bankTransaction.Category == "Bank Interest" || bankTransaction.Category == "Bank Charges")
            {
                var bankAccountPosting = new BankAccountPosting
                {
                    TransactionPersonFirstName = bankTransaction.TransactionPersonFirstName,
                    TransactionPersonMiddleName = bankTransaction.TransactionPersonMiddleName,
                    TransactionPersonLastName = bankTransaction.TransactionPersonLastName,
                    TransactionType = bankTransaction.TransactionType,
                    Category = bankTransaction.Category,
                    Amount = bankTransaction.Amount,
                    TransactionDate = bankTransaction.TransactionDate,
                    PaymentMethodID = bankTransaction.PaymentMethodID,
                    BankAccountID = bankTransaction.BankAccountID
                };
                _logger.Information("Transaction Created For Bank Acoount Posting");
                await _bankAccountPostingRepository.AddAsync(bankAccountPosting);
            }

            var bankAccount = await _bankAccountRepository.GetByIdAsync(bankTransaction.BankAccountID);
            if (bankAccount != null)
            {
                if (bankTransaction.TransactionType == "Credit")
                {
                    bankAccount.TotalBalance += bankTransaction.Amount;
                }
                else if (bankTransaction.TransactionType == "Debit")
                {
                    bankAccount.TotalBalance -= bankTransaction.Amount;
                }
                _logger.Information("Total Balance Updated");
                await _bankAccountRepository.UpdateAsync(bankAccount);
            }
        }

        public async Task UpdateBankTransactionAsync(Guid id, BankTransaction updatedBankTransaction)
        {
            var existingBankTransaction = await _bankTransactionRepository.GetByIdAsync(id);
            if (existingBankTransaction == null)
            {
                _logger.Error($"Bank transaction not found for Id{id}");
                throw new ArgumentException("Bank transaction not found."); 
            }
               

            existingBankTransaction.TransactionPersonFirstName = updatedBankTransaction.TransactionPersonFirstName;
            existingBankTransaction.TransactionPersonMiddleName = updatedBankTransaction.TransactionPersonMiddleName;
            existingBankTransaction.TransactionPersonLastName = updatedBankTransaction.TransactionPersonLastName;
            existingBankTransaction.TransactionType = updatedBankTransaction.TransactionType;
            existingBankTransaction.Category = updatedBankTransaction.Category;
            existingBankTransaction.Amount = updatedBankTransaction.Amount;
            existingBankTransaction.TransactionDate = updatedBankTransaction.TransactionDate;

            _logger.Information($"Bank Transaction Updated For Id {id}");
            await _bankTransactionRepository.UpdateAsync(existingBankTransaction);
        }

        public async Task DeleteBankTransactionAsync(Guid id)
        {
            var bankTransaction = await _bankTransactionRepository.GetByIdAsync(id);
            if (bankTransaction == null) {
                _logger.Error($"Bank Transaction Not Found For Id {id}");
                throw new ArgumentException("Bank transaction not found.");
            }
            _logger.Information($"Delete bank transaction for Id {id}");
            await _bankTransactionRepository.DeleteAsync(bankTransaction);
        }

        public async Task SaveChangesAsync()
        {
            _logger.Information("Saving Changes");
            await _unitOfWork.SaveAsync();
        }
    }
}
