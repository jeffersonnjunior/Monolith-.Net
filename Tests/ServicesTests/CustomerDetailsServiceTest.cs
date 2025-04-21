using Application.Dtos;
using Application.Interfaces.IFactory;
using Application.Services;
using Domain.Entities;
using Infrastructure.Interfaces.IRepositories;
using Infrastructure.Notifications;
using Infrastructure.FiltersModel;
using Moq;

public class CustomerDetailsServiceTest
{
    private readonly Mock<ICustomerDetailsRepository> _customerDetailsRepositoryMock;
    private readonly Mock<ICustomerDetailsFactory> _customerDetailsFactoryMock;
    private readonly Mock<NotificationContext> _notificationContextMock;
    private readonly CustomerDetailsService _customerDetailsService;

    public CustomerDetailsServiceTest()
    {
        _customerDetailsRepositoryMock = new Mock<ICustomerDetailsRepository>();
        _customerDetailsFactoryMock = new Mock<ICustomerDetailsFactory>();
        _notificationContextMock = new Mock<NotificationContext>();
        _customerDetailsService = new CustomerDetailsService(
            _customerDetailsRepositoryMock.Object,
            _customerDetailsFactoryMock.Object,
            _notificationContextMock.Object
        );
    }

    [Fact]
    public void GetById_ShouldReturnCustomerDetailsReadDto_WhenCustomerExists()
    {
        // Arrange
        var filter = new FilterCustomerDetailsById { Id = Guid.NewGuid() };
        var customerDetails = new CustomerDetails
        {
            Id = Guid.NewGuid(),
            Name = "John Doe",
            Email = "johndoe@example.com",
            Age = 30,
            Tickets = new List<Tickets>()
        };
        var customerDetailsReadDto = new CustomerDetailsReadDto
        {
            Name = "John Doe",
            Email = "johndoe@example.com"
        };

        _customerDetailsRepositoryMock
            .Setup(repo => repo.GetByElement(It.IsAny<FilterByItem>()))
            .Returns(customerDetails);
        _customerDetailsFactoryMock
            .Setup(factory => factory.MapToCustomerDetailsReadDto(customerDetails))
            .Returns(customerDetailsReadDto);

        // Act
        var result = _customerDetailsService.GetById(filter);

        // Assert
        Assert.Equal(customerDetailsReadDto, result);
    }

    [Fact]
    public void GetFilter_ShouldReturnFilterReturnOfCustomerDetailsReadDto()
    {
        // Arrange
        var filter = new FilterCustomerDetailsTable();
        var filterResult = new FilterReturn<CustomerDetails>
        {
            ItensList = new List<CustomerDetails>
            {
                new CustomerDetails
                {
                    Id = Guid.NewGuid(),
                    Name = "John Doe",
                    Email = "johndoe@example.com",
                    Age = 30,
                    Tickets = new List<Tickets>()
                }
            },
            TotalRegister = 10,
            TotalRegisterFilter = 5,
            TotalPages = 1
        };

        _customerDetailsRepositoryMock
            .Setup(repo => repo.GetFilter(filter))
            .Returns(filterResult);
        _customerDetailsFactoryMock
            .Setup(factory => factory.MapToCustomerDetailsReadDto(It.IsAny<CustomerDetails>()))
            .Returns(new CustomerDetailsReadDto
            {
                Name = "John Doe",
                Email = "johndoe@example.com"
            });

        // Act
        var result = _customerDetailsService.GetFilter(filter);

        // Assert
        Assert.Equal(filterResult.TotalRegister, result.TotalRegister);
        Assert.Equal(filterResult.TotalRegisterFilter, result.TotalRegisterFilter);
        Assert.Equal(filterResult.TotalPages, result.TotalPages);
    }

    [Fact]
    public void Add_ShouldReturnCustomerDetailsUpdateDto_WhenValidInput()
    {
        // Arrange
        var customerDetailsCreateDto = new CustomerDetailsCreateDto
        {
            Name = "John Doe",
            Email = "johndoe@example.com"
        };
        var customerDetails = new CustomerDetails
        {
            Id = Guid.NewGuid(),
            Name = "John Doe",
            Email = "johndoe@example.com",
            Age = 30,
            Tickets = new List<Tickets>()
        };
        var customerDetailsUpdateDto = new CustomerDetailsUpdateDto
        {
            Name = "John Doe",
            Email = "johndoe@example.com"
        };

        _customerDetailsRepositoryMock
            .Setup(repo => repo.ValidateInput(customerDetailsCreateDto, false, null))
            .Returns(true);
        _customerDetailsFactoryMock
            .Setup(factory => factory.MapToCustomerDetails(customerDetailsCreateDto))
            .Returns(customerDetails);
        _customerDetailsRepositoryMock
            .Setup(repo => repo.Add(customerDetails))
            .Returns(customerDetails);
        _customerDetailsFactoryMock
            .Setup(factory => factory.MapToCustomerDetailsUpdateDto(customerDetails))
            .Returns(customerDetailsUpdateDto);

        // Act
        var result = _customerDetailsService.Add(customerDetailsCreateDto);

        // Assert
        Assert.Equal(customerDetailsUpdateDto.Name, result.Name);
        Assert.Equal(customerDetailsUpdateDto.Email, result.Email);
    }

    [Fact]
    public void Update_ShouldCallRepositoryUpdate_WhenValidInput()
    {
        // Arrange
        var customerDetailsUpdateDto = new CustomerDetailsUpdateDto
        {
            Id = Guid.NewGuid(),
            Name = "John Doe",
            Email = "johndoe@example.com"
        };
        var customerDetails = new CustomerDetails
        {
            Id = customerDetailsUpdateDto.Id,
            Name = "John Doe",
            Email = "johndoe@example.com",
            Age = 30,
            Tickets = new List<Tickets>()
        };

        _customerDetailsRepositoryMock
            .Setup(repo => repo.GetByElement(It.IsAny<FilterByItem>()))
            .Returns(customerDetails);
        _customerDetailsRepositoryMock
            .Setup(repo => repo.ValidateInput(customerDetailsUpdateDto, false, customerDetails))
            .Returns(true);
        _customerDetailsFactoryMock
            .Setup(factory => factory.MapToCustomerDetailsFromUpdateDto(customerDetailsUpdateDto))
            .Returns(customerDetails);

        // Act
        _customerDetailsService.Update(customerDetailsUpdateDto);

        // Assert
        _customerDetailsRepositoryMock.Verify(repo => repo.Update(customerDetails), Times.Once);
    }

    [Fact]
    public void Delete_ShouldCallRepositoryDelete_WhenCustomerExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var customerDetails = new CustomerDetails
        {
            Id = id,
            Name = "John Doe",
            Email = "johndoe@example.com",
            Age = 30,
            Tickets = new List<Tickets>()
        };

        _customerDetailsRepositoryMock
            .Setup(repo => repo.GetByElement(It.IsAny<FilterByItem>()))
            .Returns(customerDetails);

        // Act
        _customerDetailsService.Delete(id);

        // Assert
        _customerDetailsRepositoryMock.Verify(repo => repo.Delete(customerDetails), Times.Once);
    }

    [Fact]
    public void Delete_ShouldNotCallRepositoryDelete_WhenCustomerDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();

        _customerDetailsRepositoryMock
            .Setup(repo => repo.GetByElement(It.IsAny<FilterByItem>()))
            .Returns((CustomerDetails)null);

        // Act
        _customerDetailsService.Delete(id);

        // Assert
        _customerDetailsRepositoryMock.Verify(repo => repo.Delete(It.IsAny<CustomerDetails>()), Times.Never);
    }
}