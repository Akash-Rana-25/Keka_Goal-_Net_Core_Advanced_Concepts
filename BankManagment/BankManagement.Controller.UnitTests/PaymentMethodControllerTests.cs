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
    public class PaymentMethodControllerTests : BaseTest
    {
        [Fact]
        public async Task GetPaymentMethods_ReturnsOkResultWithPaymentMethods()
        {
            // Arrange
            var fixture = new Fixture();
            var mockPaymentMethodService = new Mock<IPaymentMethodService>();
            var mockMapper = new Mock<IMapper>();
            var controller = new PaymentMethodController(mockPaymentMethodService.Object, mockMapper.Object);

            var paymentMethodDTOs = fixture.CreateMany<PaymentMethodDTO>().ToList();
            var paymentMethods = fixture.CreateMany<PaymentMethod>().ToList();

            mockPaymentMethodService.Setup(service => service.GetAllPaymentMethodsAsync()).ReturnsAsync(paymentMethods);
            mockMapper.Setup(mapper => mapper.Map<List<PaymentMethodDTO>>(paymentMethods)).Returns(paymentMethodDTOs);

            // Act
            var result = await controller.GetPaymentMethods() as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Same(paymentMethodDTOs, result.Value);
        }

        [Fact]
        public async Task UpdatePaymentMethod_ReturnsNoContentWhenModelStateIsValid()
        {
            // Arrange
            var fixture = new Fixture();
            var paymentMethodId = fixture.Create<Guid>();
            var mockPaymentMethodService = new Mock<IPaymentMethodService>();
            var mockMapper = new Mock<IMapper>();
            var controller = new PaymentMethodController(mockPaymentMethodService.Object, mockMapper.Object);

            var updatedPaymentMethodDTO = fixture.Create<PaymentMethodDTO>();
            updatedPaymentMethodDTO.Id = paymentMethodId;
            var updatedPaymentMethod = fixture.Create<PaymentMethod>();
            updatedPaymentMethod.Id = paymentMethodId;

            mockMapper.Setup(mapper => mapper.Map<PaymentMethod>(updatedPaymentMethodDTO)).Returns(updatedPaymentMethod);
            mockPaymentMethodService.Setup(service => service.UpdatePaymentMethodAsync(paymentMethodId, updatedPaymentMethod)).Returns(Task.CompletedTask);
            mockPaymentMethodService.Setup(service => service.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await controller.UpdatePaymentMethod(paymentMethodId, updatedPaymentMethodDTO) as NoContentResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(204, result.StatusCode);
        }


        [Fact]
        public async Task DeletePaymentMethod_ReturnsNoContent()
        {
            // Arrange
            var fixture = new Fixture();
            var paymentMethodId = fixture.Create<Guid>();
            var mockPaymentMethodService = new Mock<IPaymentMethodService>();
            var controller = new PaymentMethodController(mockPaymentMethodService.Object, null);

            mockPaymentMethodService.Setup(service => service.DeletePaymentMethodAsync(paymentMethodId)).Returns(Task.CompletedTask);
            mockPaymentMethodService.Setup(service => service.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await controller.DeletePaymentMethod(paymentMethodId) as NoContentResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(204, result.StatusCode);
        }

        [Fact]
        public async Task CreatePaymentMethod_ReturnsBadRequestWhenModelStateIsInvalid()
        {
            // Arrange
            var fixture = new Fixture();
            var controller = new PaymentMethodController(null, null);
            controller.ModelState.AddModelError("Name", "Name is required");

            // Act
            var result = await controller.CreatePaymentMethod(null);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task UpdatePaymentMethod_ReturnsBadRequestWhenModelStateIsInvalid()
        {
            // Arrange
            var fixture = new Fixture();
            var controller = new PaymentMethodController(null, null);
            controller.ModelState.AddModelError("Name", "Name is required");

            // Act
            var result = await controller.UpdatePaymentMethod(fixture.Create<Guid>(), null);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }
    }
}
