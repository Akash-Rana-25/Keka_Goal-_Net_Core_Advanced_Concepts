using BankManagment_Domain.Entity;
using BankManagment_Infrastructure.Repository;
using BankManagment_Services;
using Moq;
using AutoFixture;

namespace BankManagement.Services.UnitTests
{
    public class PaymentMethodServiceTests : BaseTest
    {
        [Fact]
        public async Task GetAllPaymentMethodsAsync_ReturnsPaymentMethods()
        {
            // Arrange
            var mockPaymentMethodRepository = new Mock<IRepository<PaymentMethod>>();
            var paymentMethods = Fixture.CreateMany<PaymentMethod>(2);
            mockPaymentMethodRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(paymentMethods);

            var service = new PaymentMethodService(null, mockPaymentMethodRepository.Object);

            // Act
            var result = await service.GetAllPaymentMethodsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task CreatePaymentMethodAsync_CreatesNewPaymentMethod()
        {
            // Arrange
            var mockPaymentMethodRepository = new Mock<IRepository<PaymentMethod>>();
            var newPaymentMethod = Fixture.Create<PaymentMethod>();

            mockPaymentMethodRepository.Setup(repo => repo.AddAsync(newPaymentMethod)).Returns(Task.CompletedTask);

            var service = new PaymentMethodService(null, mockPaymentMethodRepository.Object);

            // Act
            await service.CreatePaymentMethodAsync(newPaymentMethod);

            // Assert
            mockPaymentMethodRepository.Verify(repo => repo.AddAsync(newPaymentMethod), Times.Once);
        }
        [Fact]
        public async Task UpdatePaymentMethodAsync_UpdatesExistingPaymentMethod()
        {
            // Arrange
            var paymentMethodId = Fixture.Create<Guid>();
            var updatedPaymentMethod = Fixture.Build<PaymentMethod>()
                .With(p => p.Name, "Updated Payment Method")
                .Create();

            var mockPaymentMethodRepository = new Mock<IRepository<PaymentMethod>>();

            // Set up the GetByIdAsync to return the updatedPaymentMethod with a matching Id
            mockPaymentMethodRepository.Setup(repo => repo.GetByIdAsync(paymentMethodId))
                .ReturnsAsync(updatedPaymentMethod);

            // Set up the UpdateAsync method to return a completed task
            mockPaymentMethodRepository.Setup(repo => repo.UpdateAsync(updatedPaymentMethod))
                .Returns(Task.CompletedTask);

            var service = new PaymentMethodService(null, mockPaymentMethodRepository.Object);

            // Act
            await service.UpdatePaymentMethodAsync(paymentMethodId, updatedPaymentMethod);

            // Assert
            mockPaymentMethodRepository.Verify(repo => repo.UpdateAsync(updatedPaymentMethod), Times.Once);
        }



        [Fact]
        public async Task DeletePaymentMethodAsync_DeletesExistingPaymentMethod()
        {
            // Arrange
            var paymentMethodId = Fixture.Create<Guid>();

            var mockPaymentMethodRepository = new Mock<IRepository<PaymentMethod>>();
            mockPaymentMethodRepository.Setup(repo => repo.GetByIdAsync(paymentMethodId))
                .ReturnsAsync(Fixture.Create<PaymentMethod>());

            var service = new PaymentMethodService(null, mockPaymentMethodRepository.Object);

            // Act
            await service.DeletePaymentMethodAsync(paymentMethodId);

            // Assert
            mockPaymentMethodRepository.Verify(repo => repo.DeleteAsync(It.IsAny<PaymentMethod>()), Times.Once);
        }
    }
}
