using BankManagment_Domain.Entity;
using BankManagment_Infrastructure.Repository;
using BankManagment_Services;
using Moq;
using AutoFixture;

namespace BankManagement.Services.UnitTests
{
    public class BankTransactionServiceTests : BaseTest
    {
        [Fact]
        public async Task GetAllBankTransactionsAsync_ReturnsBankTransactions()
        {
            // Arrange
            var mockBankTransactionRepository = new Mock<IRepository<BankTransaction>>();
            var bankTransactions = Fixture.CreateMany<BankTransaction>(2);
            mockBankTransactionRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(bankTransactions);

            var service = new BankTransactionService(null, mockBankTransactionRepository.Object, null, null);

            // Act
            var result = await service.GetAllBankTransactionsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task CreateBankTransactionAsync_CreatesNewBankTransaction()
        {
            // Arrange
            var fixture = new Fixture();
            var mockBankTransactionRepository = new Mock<IRepository<BankTransaction>>();
            var mockBankAccountPostingRepository = new Mock<IRepository<BankAccountPosting>>();
            var mockBankAccountRepository = new Mock<IRepository<BankAccount>>();

            var newBankTransaction = fixture.Create<BankTransaction>();
            newBankTransaction.Id = fixture.Create<Guid>();
            newBankTransaction.TransactionType = "Credit";
            newBankTransaction.Amount = 100;
            newBankTransaction.Category = "Bank Interest";
            newBankTransaction.PaymentMethodID = fixture.Create<Guid>();
            newBankTransaction.BankAccountID = fixture.Create<Guid>();

            mockBankTransactionRepository.Setup(repo => repo.AddAsync(newBankTransaction)).Returns(Task.CompletedTask);

            var existingBankAccount = fixture.Create<BankAccount>();
            existingBankAccount.Id = newBankTransaction.BankAccountID;
            existingBankAccount.TotalBalance = 200;

            mockBankAccountRepository.Setup(repo => repo.GetByIdAsync(newBankTransaction.BankAccountID))
                .ReturnsAsync(existingBankAccount);

            var service = new BankTransactionService(null, mockBankTransactionRepository.Object, mockBankAccountPostingRepository.Object, mockBankAccountRepository.Object);

            // Act
            await service.CreateBankTransactionAsync(newBankTransaction);

            // Assert
            mockBankTransactionRepository.Verify(repo => repo.AddAsync(newBankTransaction), Times.Once);
            mockBankAccountRepository.Verify(repo => repo.UpdateAsync(existingBankAccount), Times.Once);
            Assert.Equal(300, existingBankAccount.TotalBalance);
        }


        [Fact]
        public async Task UpdateBankTransactionAsync_UpdatesExistingBankTransaction()
        {
            // Arrange
            var bankTransactionId = Fixture.Create<Guid>();
            var updatedBankTransaction = Fixture.Create<BankTransaction>();

            var mockBankTransactionRepository = new Mock<IRepository<BankTransaction>>();
            mockBankTransactionRepository.Setup(repo => repo.GetByIdAsync(bankTransactionId))
                .ReturnsAsync(updatedBankTransaction);

            // Setup the UpdateAsync method
            mockBankTransactionRepository.Setup(repo => repo.UpdateAsync(updatedBankTransaction))
                .Returns(Task.CompletedTask);

            var service = new BankTransactionService(null, mockBankTransactionRepository.Object, null, null);

            // Act
            await service.UpdateBankTransactionAsync(bankTransactionId, updatedBankTransaction);

            // Assert
            mockBankTransactionRepository.Verify(repo => repo.UpdateAsync(updatedBankTransaction), Times.Once);
        }


        [Fact]
        public async Task DeleteBankTransactionAsync_DeletesExistingBankTransaction()
        {
            // Arrange
            var bankTransactionId = Fixture.Create<Guid>();

            var mockBankTransactionRepository = new Mock<IRepository<BankTransaction>>();
            mockBankTransactionRepository.Setup(repo => repo.GetByIdAsync(bankTransactionId))
                .ReturnsAsync(Fixture.Create<BankTransaction>());

            var service = new BankTransactionService(null, mockBankTransactionRepository.Object, null, null);

            // Act
            await service.DeleteBankTransactionAsync(bankTransactionId);

            // Assert
            mockBankTransactionRepository.Verify(repo => repo.DeleteAsync(It.IsAny<BankTransaction>()), Times.Once);
        }
    }
}
