using Domain.Entities;
using Infrastructure.Interfaces.IRepositories;
using Infrastructure.Notifications;
using Infrastructure.Utilities.FiltersModel;
using Moq;

namespace RepositoryTest;

public class CustomerDetailsRepositoryTest
{
    private readonly Mock<ICustomerDetailsRepository> _customerDetailsRepositoryMock;
    private readonly Mock<NotificationContext> _notificationContextMock;

    public CustomerDetailsRepositoryTest()
    {
        _customerDetailsRepositoryMock = new Mock<ICustomerDetailsRepository>();
        _notificationContextMock = new Mock<NotificationContext>();
    }

    [Fact]
    public void GetByElement_ShouldReturnCustomerDetails()
    {
        // Arrange
        var filterByItem = new FilterByItem { Field = "Id", Value = Guid.NewGuid(), Key = "Equal" };
        var customerDetails = new CustomerDetails { Name = "John Doe", Email = "john.doe@example.com" };

        _customerDetailsRepositoryMock.Setup(repo => repo.GetByElement(It.IsAny<FilterByItem>())).Returns(customerDetails);

        // Act
        var result = _customerDetailsRepositoryMock.Object.GetByElement(filterByItem);

        // Assert
        Assert.Equal(customerDetails, result);
    }

    [Fact]
    public void GetFilter_ShouldReturnFilterReturnOfCustomerDetails()
    {
        // Arrange
        var filter = new FilterCustomerDetailsTable();
        var filterResult = new FilterReturn<CustomerDetails> { ItensList = new List<CustomerDetails>() };

        _customerDetailsRepositoryMock.Setup(repo => repo.GetFilter(filter)).Returns(filterResult);

        // Act
        var result = _customerDetailsRepositoryMock.Object.GetFilter(filter);

        // Assert
        Assert.Equal(filterResult.ItensList, result.ItensList);
    }

    [Fact]
    public void ValidateInput_ShouldReturnTrue_WhenEmailIsNotInUse()
    {
        // Arrange
        var customerDetailsCreateDto = new { Email = "john.doe@example.com" };
        var existingCustomerDetails = new CustomerDetails { Name = "John Doe", Email = "jane.doe@example.com" };

        _customerDetailsRepositoryMock.Setup(repo => repo.ValidateInput(customerDetailsCreateDto, false, existingCustomerDetails)).Returns(true);

        // Act
        var result = _customerDetailsRepositoryMock.Object.ValidateInput(customerDetailsCreateDto, false, existingCustomerDetails);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void ValidateInput_ShouldReturnFalse_WhenEmailIsInUse()
    {
        // Arrange
        var customerDetailsCreateDto = new { Email = "john.doe@example.com" };
        var existingCustomerDetails = new CustomerDetails { Name = "John Doe", Email = "john.doe@example.com" };

        _customerDetailsRepositoryMock.Setup(repo => repo.ValidateInput(customerDetailsCreateDto, false, existingCustomerDetails)).Returns(false);

        // Act
        var result = _customerDetailsRepositoryMock.Object.ValidateInput(customerDetailsCreateDto, false, existingCustomerDetails);

        // Assert
        Assert.False(result);
    }
}