using AutoFixture;
using AutoMapper;
using BankManagment_DTO;
using BankManagment_Domain.Entity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using BankManagment_Services;
using BankManagement.Controller.UnitTests;

namespace BankManagement.UnitTests.ControllerUnitTests
{
    public class BankAccountControllerTests : BaseTest
    {
        [Fact]
        public async Task GetBankAccounts_ReturnsOkResultWithBankAccounts()
        {
            // Arrange
            var fixture = new Fixture();
            var mockBankAccountService = new Mock<IBankAccountService>();
            var mockMapper = new Mock<IMapper>();
            var controller = new BankAccountController(mockBankAccountService.Object, mockMapper.Object);

            var bankAccountDTOs = fixture.CreateMany<BankAccountDTO>().ToList();
            var bankAccounts = fixture.CreateMany<BankAccount>().ToList();

            mockBankAccountService.Setup(service => service.GetAllBankAccountsAsync()).ReturnsAsync(bankAccounts);
            mockMapper.Setup(mapper => mapper.Map<List<BankAccountDTO>>(bankAccounts)).Returns(bankAccountDTOs);

            // Act
            var result = await controller.GetBankAccounts() as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Same(bankAccountDTOs, result.Value);
        }

        [Fact]
        public async Task UpdateBankAccount_ReturnsNoContentWhenModelStateIsValid()
        {
            // Arrange
            var fixture = new Fixture();
            var accountId = fixture.Create<Guid>();
            var mockBankAccountService = new Mock<IBankAccountService>();
            var mockMapper = new Mock<IMapper>();
            var controller = new BankAccountController(mockBankAccountService.Object, mockMapper.Object);

            var updatedBankAccountDTO = fixture.Create<BankAccountDTO>();
            updatedBankAccountDTO.Id = accountId;
            var updatedBankAccount = fixture.Create<BankAccount>();
            updatedBankAccount.Id = accountId;

            mockMapper.Setup(mapper => mapper.Map<BankAccount>(updatedBankAccountDTO)).Returns(updatedBankAccount);
            mockBankAccountService.Setup(service => service.UpdateBankAccountAsync(accountId, updatedBankAccount)).Returns(Task.CompletedTask);
            mockBankAccountService.Setup(service => service.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await controller.UpdateBankAccount(accountId, updatedBankAccountDTO) as NoContentResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(204, result.StatusCode);
        }

        [Fact]
        public async Task DeleteBankAccount_ReturnsNoContent()
        {
            // Arrange
            var fixture = new Fixture();
            var accountId = fixture.Create<Guid>();
            var mockBankAccountService = new Mock<IBankAccountService>();
            var controller = new BankAccountController(mockBankAccountService.Object, null);

            mockBankAccountService.Setup(service => service.DeleteBankAccountAsync(accountId)).Returns(Task.CompletedTask);
            mockBankAccountService.Setup(service => service.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await controller.DeleteBankAccount(accountId) as NoContentResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(204, result.StatusCode);
        }

        [Fact]
        public async Task CreateBankAccount_ReturnsBadRequestWhenModelStateIsInvalid()
        {
            // Arrange
            var fixture = new Fixture();
            var controller = new BankAccountController(null, null);
            controller.ModelState.AddModelError("FirstName", "FirstName is required");

            // Act
            var result = await controller.CreateBankAccount(null);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task UpdateBankAccount_ReturnsBadRequestWhenModelStateIsInvalid()
        {
            // Arrange
            var fixture = new Fixture();
            var controller = new BankAccountController(null, null);
            controller.ModelState.AddModelError("LastName", "LastName is required");

            // Act
            var result = await controller.UpdateBankAccount(fixture.Create<Guid>(), null);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }
    }
}
