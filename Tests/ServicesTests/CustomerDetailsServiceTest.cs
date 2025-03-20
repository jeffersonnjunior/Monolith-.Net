using Application.Dtos;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces.IRepositories;
using Infrastructure.Notifications;
using Infrastructure.Utilities.FiltersModel;
using Moq;

namespace ServicesTest;

public class CustomerDetailsServiceTest
{
    private readonly Mock<ICustomerDetailsRepository> _customerDetailsRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<NotificationContext> _notifierContextMock;

    public CustomerDetailsServiceTest()
    {
        _customerDetailsRepositoryMock = new Mock<ICustomerDetailsRepository>();
        _mapperMock = new Mock<IMapper>();
        _notifierContextMock = new Mock<NotificationContext>();
    }

    [Fact]
    public void GetById_ShouldReturnCustomerDetailsReadDto()
    {
        // Arrange
        var filter = new FilterCustomerDetailsById { Id = Guid.NewGuid() };
        var customerDetails = new CustomerDetails { Name = "John Doe", Email = "john.doe@example.com" };
        var customerDetailsReadDto = new CustomerDetailsReadDto { Name = "John Doe", Email = "john.doe@example.com" };

        _customerDetailsRepositoryMock.Setup(repo => repo.GetByElement(It.IsAny<FilterByItem>())).Returns(customerDetails);
        _mapperMock.Setup(mapper => mapper.Map<CustomerDetailsReadDto>(customerDetails)).Returns(customerDetailsReadDto);

        // Act
        var result = _mapperMock.Object.Map<CustomerDetailsReadDto>(_customerDetailsRepositoryMock.Object.GetByElement(new FilterByItem { Field = "Id", Value = filter.Id, Key = "Equal", Includes = filter.Includes }));

        // Assert
        Assert.Equal(customerDetailsReadDto, result);
    }

    [Fact]
    public void GetFilter_ShouldReturnFilterReturnOfCustomerDetailsReadDto()
    {
        // Arrange
        var filter = new FilterCustomerDetailsTable();
        var filterResult = new FilterReturn<CustomerDetails> { ItensList = new List<CustomerDetails>() };
        var customerDetailsReadDtoList = new List<CustomerDetailsReadDto>();

        _customerDetailsRepositoryMock.Setup(repo => repo.GetFilter(filter)).Returns(filterResult);
        _mapperMock.Setup(mapper => mapper.Map<IEnumerable<CustomerDetailsReadDto>>(filterResult.ItensList)).Returns(customerDetailsReadDtoList);

        // Act
        var result = new FilterReturn<CustomerDetailsReadDto>
        {
            TotalRegister = filterResult.TotalRegister,
            TotalRegisterFilter = filterResult.TotalRegisterFilter,
            TotalPages = filterResult.TotalPages,
            ItensList = _mapperMock.Object.Map<IEnumerable<CustomerDetailsReadDto>>(filterResult.ItensList)
        };

        // Assert
        Assert.Equal(customerDetailsReadDtoList, result.ItensList);
    }

    [Fact]
    public void Add_ShouldReturnCustomerDetailsUpdateDto()
    {
        // Arrange
        var customerDetailsCreateDto = new CustomerDetailsCreateDto { Name = "John Doe", Email = "john.doe@example.com", Age = 20 };
        var customerDetails = new CustomerDetails { Name = "John Doe", Email = "john.doe@example.com", Age = 20 };
        var customerDetailsUpdateDto = new CustomerDetailsUpdateDto { Name = "John Doe", Email = "john.doe@example.com", Age = 20 };

        _customerDetailsRepositoryMock.Setup(repo => repo.ValidateInput(customerDetailsCreateDto, false, customerDetails)).Returns(true);
        _mapperMock.Setup(mapper => mapper.Map<CustomerDetails>(customerDetailsCreateDto)).Returns(customerDetails);
        _mapperMock.Setup(mapper => mapper.Map<CustomerDetailsUpdateDto>(customerDetails)).Returns(customerDetailsUpdateDto);
        _customerDetailsRepositoryMock.Setup(repo => repo.Add(customerDetails)).Returns(customerDetails);

        // Act
        var result = _mapperMock.Object.Map<CustomerDetailsUpdateDto>(_customerDetailsRepositoryMock.Object.Add(customerDetails));

        // Assert
        Assert.Equal(customerDetailsUpdateDto.Name, result.Name);
        Assert.Equal(customerDetailsUpdateDto.Email, result.Email);
        Assert.Equal(customerDetailsUpdateDto.Age, result.Age);
    }

    [Fact]
    public void Update_ShouldCallRepositoryUpdate()
    {
        // Arrange
        var customerDetailsUpdateDto = new CustomerDetailsUpdateDto { Id = Guid.NewGuid(), Name = "John Doe", Email = "john.doe@example.com" };
        var customerDetails = new CustomerDetails { Name = "John Doe", Email = "john.doe@example.com" };

        _customerDetailsRepositoryMock.Setup(repo => repo.GetByElement(It.IsAny<FilterByItem>())).Returns(customerDetails);
        _customerDetailsRepositoryMock.Setup(repo => repo.ValidateInput(customerDetailsUpdateDto, false, customerDetails)).Returns(true);
        _mapperMock.Setup(mapper => mapper.Map<CustomerDetails>(customerDetailsUpdateDto)).Returns(customerDetails);

        // Act
        _customerDetailsRepositoryMock.Object.Update(customerDetails);

        // Assert
        _customerDetailsRepositoryMock.Verify(repo => repo.Update(customerDetails), Times.Once);
    }

    [Fact]
    public void Delete_ShouldCallRepositoryDelete()
    {
        // Arrange
        var id = Guid.NewGuid();
        var customerDetails = new CustomerDetails { Name = "John Doe", Email = "john.doe@example.com" };

        _customerDetailsRepositoryMock.Setup(repo => repo.GetByElement(It.IsAny<FilterByItem>())).Returns(customerDetails);

        // Act
        _customerDetailsRepositoryMock.Object.Delete(customerDetails);

        // Assert
        _customerDetailsRepositoryMock.Verify(repo => repo.Delete(customerDetails), Times.Once);
    }
}