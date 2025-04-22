using Application.Dtos;
using Application.Interfaces.IFactories;
using Application.Services;
using Domain.Entities;
using Infrastructure.Interfaces.IRepositories;
using Infrastructure.Notifications;
using Infrastructure.FiltersModel;
using Moq;

public class TheaterLocationServiceTests
{
    private readonly Mock<ITheaterLocationRepository> _theaterLocationRepositoryMock;
    private readonly Mock<ITheaterLocationFactory> _theaterLocationFactoryMock;
    private readonly NotificationContext _notificationContext;
    private readonly TheaterLocationService _theaterLocationService;

    public TheaterLocationServiceTests()
    {
        _theaterLocationRepositoryMock = new Mock<ITheaterLocationRepository>();
        _theaterLocationFactoryMock = new Mock<ITheaterLocationFactory>();
        _notificationContext = new NotificationContext();
        _theaterLocationService = new TheaterLocationService(
            _theaterLocationRepositoryMock.Object,
            _notificationContext,
            _theaterLocationFactoryMock.Object
        );
    }

    [Fact]
    public void GetById_ShouldReturnTheaterLocationReadDto_WhenTheaterLocationExists()
    {
        // Arrange
        var locationId = Guid.NewGuid();
        var theaterLocation = new TheaterLocation
        {
            Id = locationId,
            Street = "Main St",
            UnitNumber = "123",
            PostalCode = "12345"
        };
        var theaterLocationReadDto = new TheaterLocationReadDto
        {
            Id = locationId,
            Street = "Main St",
            UnitNumber = "123",
            PostalCode = "12345"
        };

        _theaterLocationRepositoryMock
            .Setup(repo => repo.GetByElement(It.IsAny<FilterByItem>()))
            .Returns(theaterLocation);

        _theaterLocationFactoryMock
            .Setup(factory => factory.MapToTheaterLocationReadDto(theaterLocation))
            .Returns(theaterLocationReadDto);

        var filter = new FilterTheaterLocationById { Id = locationId };

        // Act
        var result = _theaterLocationService.GetById(filter);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(locationId, result.Id);
        Assert.Equal("Main St", result.Street);
    }

    [Fact]
    public void Add_ShouldReturnTheaterLocationUpdateDto_WhenInputIsValid()
    {
        // Arrange
        var createDto = new TheaterLocationCreateDto
        {
            Street = "Main St",
            UnitNumber = "123",
            PostalCode = "12345"
        };
        var theaterLocation = new TheaterLocation
        {
            Id = Guid.NewGuid(),
            Street = "Main St",
            UnitNumber = "123",
            PostalCode = "12345"
        };
        var updateDto = new TheaterLocationUpdateDto
        {
            Id = theaterLocation.Id,
            Street = "Main St",
            UnitNumber = "123",
            PostalCode = "12345"
        };

        _theaterLocationFactoryMock
            .Setup(factory => factory.MapToTheaterLocation(createDto))
            .Returns(theaterLocation);

        _theaterLocationRepositoryMock
            .Setup(repo => repo.Add(theaterLocation))
            .Returns(theaterLocation);

        _theaterLocationFactoryMock
            .Setup(factory => factory.MapToTheaterLocationUpdateDto(theaterLocation))
            .Returns(updateDto);

        // Act
        var result = _theaterLocationService.Add(createDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(theaterLocation.Id, result.Id);
        Assert.Equal("Main St", result.Street);
    }

    [Fact]
    public void Delete_ShouldCallRepositoryDelete_WhenTheaterLocationExists()
    {
        // Arrange
        var locationId = Guid.NewGuid();
        var theaterLocation = new TheaterLocation
        {
            Id = locationId,
            Street = "Main St",
            UnitNumber = "123",
            PostalCode = "12345"
        };

        _theaterLocationRepositoryMock
            .Setup(repo => repo.GetByElement(It.IsAny<FilterByItem>()))
            .Returns(theaterLocation);

        // Act
        _theaterLocationService.Delete(locationId);

        // Assert
        _theaterLocationRepositoryMock.Verify(repo => repo.Delete(theaterLocation), Times.Once);
    }
}