using AutoMapper;
using BankManagment_DTO;
using BankManagment_Domain.Entity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using BankManagment_Services;
using AutoFixture;
using BankManagement.Controller.UnitTests;

namespace BankManagement.UnitTests.ControllerUnitTests
{
    public class BankTransactionsControllerTests : BaseTest
    {
        [Fact]
        public async Task GetBankTransactions_ReturnsOkResultWithBankTransactions()
        {
            // Arrange
            var fixture = new Fixture();
            var mockBankTransactionService = new Mock<IBankTransactionService>();
            var mockMapper = new Mock<IMapper>();
            var controller = new BankTransactionsController(mockBankTransactionService.Object, mockMapper.Object);

            var bankTransactionDTOs = fixture.CreateMany<BankTransactionDTO>().ToList();
            var bankTransactions = fixture.CreateMany<BankTransaction>().ToList();

            mockBankTransactionService.Setup(service => service.GetAllBankTransactionsAsync()).ReturnsAsync(bankTransactions);
            mockMapper.Setup(mapper => mapper.Map<List<BankTransactionDTO>>(bankTransactions)).Returns(bankTransactionDTOs);

            // Act
            var result = await controller.GetBankTransactions() as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Same(bankTransactionDTOs, result.Value);
        }

        [Fact]
        public async Task UpdateBankTransaction_ReturnsNoContentWhenModelStateIsValid()
        {
            // Arrange
            var fixture = new Fixture();
            var transactionId = fixture.Create<Guid>();
            var mockBankTransactionService = new Mock<IBankTransactionService>();
            var mockMapper = new Mock<IMapper>();
            var controller = new BankTransactionsController(mockBankTransactionService.Object, mockMapper.Object);

            var updatedTransactionDTO = fixture.Create<BankTransactionDTO>();
            updatedTransactionDTO.Id = transactionId;
            var updatedTransaction = fixture.Create<BankTransaction>();
            updatedTransaction.Id = transactionId;

            mockMapper.Setup(mapper => mapper.Map<BankTransaction>(updatedTransactionDTO)).Returns(updatedTransaction);
            mockBankTransactionService.Setup(service => service.UpdateBankTransactionAsync(transactionId, updatedTransaction)).Returns(Task.CompletedTask);
            mockBankTransactionService.Setup(service => service.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await controller.UpdateBankTransaction(transactionId, updatedTransactionDTO) as NoContentResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(204, result.StatusCode);
        }


        [Fact]
        public async Task DeleteBankTransaction_ReturnsNoContent()
        {
            // Arrange
            var fixture = new Fixture();
            var transactionId = fixture.Create<Guid>();
            var mockBankTransactionService = new Mock<IBankTransactionService>();
            var controller = new BankTransactionsController(mockBankTransactionService.Object, null);

            mockBankTransactionService.Setup(service => service.DeleteBankTransactionAsync(transactionId)).Returns(Task.CompletedTask);
            mockBankTransactionService.Setup(service => service.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await controller.DeleteBankTransaction(transactionId) as NoContentResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(204, result.StatusCode);
        }

        [Fact]
        public async Task CreateBankTransaction_ReturnsBadRequestWhenModelStateIsInvalid()
        {
            // Arrange
            var fixture = new Fixture();
            var controller = new BankTransactionsController(null, null);
            controller.ModelState.AddModelError("TransactionPersonLastName", "TransactionPersonLastName is required");

            // Act
            var result = await controller.CreateBankTransaction(null);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task UpdateBankTransaction_ReturnsBadRequestWhenModelStateIsInvalid()
        {
            // Arrange
            var fixture = new Fixture();
            var controller = new BankTransactionsController(null, null);
            controller.ModelState.AddModelError("TransactionPersonLastName", "TransactionPersonLastName is required");

            // Act
            var result = await controller.UpdateBankTransaction(fixture.Create<Guid>(), null);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }
    }
}
