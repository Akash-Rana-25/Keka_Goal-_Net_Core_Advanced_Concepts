using AutoMapper;
using BankManagment_DTO;
using BankManagment_Domain.Entity;
using Moq;
using BankManagment_Services;
using Microsoft.AspNetCore.Mvc;
using AutoFixture;
using BankManagement.Controller.UnitTests;
using System.Diagnostics;

namespace BankManagement.UnitTests.ControllerUnitTests
{
    public class AccountTypeControllerTests : BaseTest
    {
        [Fact]
        public async Task GetAccountTypes_ReturnsOkResultWithAccountTypes()
        {
            // Arrange
            var fixture = new Fixture();
            var mockAccountTypeService = new Mock<IAccountTypeService>();
            var mockMapper = new Mock<IMapper>();
            var controller = new AccountTypeController(mockAccountTypeService.Object, mockMapper.Object);

            var accountTypeDTOs = fixture.CreateMany<AccountTypeDTO>().ToList();
            var accountTypes = fixture.CreateMany<AccountType>().ToList();

            mockAccountTypeService.Setup(service => service.GetAllAccountTypesAsync()).ReturnsAsync(accountTypes);
            mockMapper.Setup(mapper => mapper.Map<List<AccountTypeDTO>>(accountTypes)).Returns(accountTypeDTOs);

            // Act
            var result = await controller.GetAccountTypes() as ObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Same(accountTypeDTOs, result.Value);
        }
        [Fact]
        public async Task UpdateAccountType_ReturnsNoContentWhenModelStateIsValid()
        {
            // Arrange
            var fixture = new Fixture();
            var accountTypeId = fixture.Create<Guid>(); // Generate a random Guid
            var mockAccountTypeService = new Mock<IAccountTypeService>();
            var mockMapper = new Mock<IMapper>();
            var controller = new AccountTypeController(mockAccountTypeService.Object, mockMapper.Object);

            var updatedAccountTypeDTO = fixture.Create<AccountTypeDTO>();
            updatedAccountTypeDTO.Id = accountTypeId; // Set the Id to match accountTypeId
            var updatedAccountType = fixture.Create<AccountType>();
            updatedAccountType.Id = accountTypeId; // Set the Id to match accountTypeId

            mockMapper.Setup(mapper => mapper.Map<AccountType>(updatedAccountTypeDTO)).Returns(updatedAccountType);
            mockAccountTypeService.Setup(service => service.UpdateAccountTypeAsync(accountTypeId, updatedAccountType)).Returns(Task.CompletedTask);
            mockAccountTypeService.Setup(service => service.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await controller.UpdateAccountType(accountTypeId, updatedAccountTypeDTO) as NoContentResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(204, result.StatusCode);
        }


        [Fact]
        public async Task DeleteAccountType_ReturnsNoContent()
        {
            // Arrange
            var fixture = new Fixture();
            var accountTypeId = fixture.Create<Guid>();
            var mockAccountTypeService = new Mock<IAccountTypeService>();
            var controller = new AccountTypeController(mockAccountTypeService.Object, null);

            mockAccountTypeService.Setup(service => service.DeleteAccountTypeAsync(accountTypeId)).Returns(Task.CompletedTask);
            mockAccountTypeService.Setup(service => service.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await controller.DeleteAccountType(accountTypeId) as NoContentResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(204, result.StatusCode);
        }

        [Fact]
        public async Task CreateAccountType_ReturnsBadRequestWhenModelStateIsInvalid()
        {
            // Arrange
            var fixture = new Fixture();
            var controller = new AccountTypeController(null, null);
            controller.ModelState.AddModelError("Name", "Name is required");

            // Act
            var result = await controller.CreateAccountType(null);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task UpdateAccountType_ReturnsBadRequestWhenModelStateIsInvalid()
        {
            // Arrange
            var fixture = new Fixture();
            var controller = new AccountTypeController(null, null);
            controller.ModelState.AddModelError("Name", "Name is required");

            // Act
            var result = await controller.UpdateAccountType(fixture.Create<Guid>(), null);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }
    }
}
