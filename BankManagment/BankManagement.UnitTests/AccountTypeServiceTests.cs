using BankManagment_Domain.Entity;
using BankManagment_Infrastructure.Repository;
using BankManagment_Infrastructure.UnitOfWork;
using Moq;
using BankManagment_Services;
using AutoFixture;

namespace BankManagement.Services.UnitTests
{
    public class AccountTypeServiceTests : BaseTest
    {
        [Fact]
        public async Task GetAllAccountTypesAsync_ReturnsAccountTypes()
        {
            // Arrange
            var fixture = new Fixture();
            var mockRepository = new Mock<IRepository<AccountType>>();
            var accountTypes = fixture.CreateMany<AccountType>().ToList();
            mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(accountTypes);

            var service = new AccountTypeService(null, mockRepository.Object);

            // Act
            var result = await service.GetAllAccountTypesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(accountTypes.Count, result.Count());
        }

        [Fact]
        public async Task CreateAccountTypeAsync_CreatesNewAccountType()
        {
            // Arrange
            var fixture = new Fixture();
            var mockRepository = new Mock<IRepository<AccountType>>();
            var newAccountType = fixture.Create<AccountType>();

            var service = new AccountTypeService(null, mockRepository.Object);

            // Act
            await service.CreateAccountTypeAsync(newAccountType);

            // Assert
            mockRepository.Verify(repo => repo.AddAsync(newAccountType), Times.Once);
        }

        [Fact]
        public async Task UpdateAccountTypeAsync_UpdatesExistingAccountType()
        {
            // Arrange
            var fixture = new Fixture();
            var accountId = fixture.Create<Guid>();
            var updatedAccountType = fixture.Create<AccountType>();

            var mockRepository = new Mock<IRepository<AccountType>>();
            mockRepository.Setup(repo => repo.GetByIdAsync(accountId)).ReturnsAsync(new AccountType());

            var service = new AccountTypeService(null, mockRepository.Object);

            // Act
            await service.UpdateAccountTypeAsync(accountId, updatedAccountType);

            // Assert
            mockRepository.Verify(repo => repo.UpdateAsync(It.IsAny<AccountType>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAccountTypeAsync_DeletesExistingAccountType()
        {
            // Arrange
            var fixture = new Fixture();
            var accountId = fixture.Create<Guid>();

            var mockRepository = new Mock<IRepository<AccountType>>();
            mockRepository.Setup(repo => repo.GetByIdAsync(accountId)).ReturnsAsync(new AccountType());

            var service = new AccountTypeService(null, mockRepository.Object);

            // Act
            await service.DeleteAccountTypeAsync(accountId);

            // Assert
            mockRepository.Verify(repo => repo.DeleteAsync(It.IsAny<AccountType>()), Times.Once);
        }

        [Fact]
        public async Task SaveChangesAsync_ThrowsException_WhenSaveFails()
        {
            // Arrange
            var fixture = new Fixture();
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(uow => uow.SaveAsync()).ThrowsAsync(new Exception("Database save failed"));

            var service = new AccountTypeService(mockUnitOfWork.Object, null);

            // Act and Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => service.SaveChangesAsync());
            Assert.Equal("Database save failed", exception.Message);
        }
    }
}
