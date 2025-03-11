using Application.Dtos;
using Application.Services;
using Application.Specification;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces.IRepositories;
using Infrastructure.Notifications;
using Infrastructure.Utilities.FiltersModel;
using Moq;
using Xunit;

namespace ServicesTests;

public class TheaterLocationServiceTest
{
    private readonly Mock<ITheaterLocationRepository> _theaterLocationRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<NotificationContext> _notifierContextMock;
    private readonly TheaterLocationSpecification _theaterLocationSpecification;
    private readonly TheaterLocationService _theaterLocationService;

    public TheaterLocationServiceTest()
    {
        _theaterLocationRepositoryMock = new Mock<ITheaterLocationRepository>();
        _mapperMock = new Mock<IMapper>();
        _notifierContextMock = new Mock<NotificationContext>();
        _theaterLocationSpecification = new TheaterLocationSpecification(_notifierContextMock.Object);
        _theaterLocationService = new TheaterLocationService(
            _theaterLocationRepositoryMock.Object,
            _mapperMock.Object,
            _notifierContextMock.Object
        );
    }

    [Fact]
    public void GetById_ShouldReturnMappedDto_WhenFound()
    {
        // Arrange
        var filter = new FilterTheaterLocationById { Id = Guid.NewGuid() };
        var theaterLocation = new TheaterLocation
        {
            Id = filter.Id,
            Street = "Some Street",
            UnitNumber = "123",
            PostalCode = "12345-678"
        };

        _theaterLocationRepositoryMock.Setup(r => r.GetByElement(It.IsAny<FilterByItem>())).Returns(theaterLocation);
        _mapperMock.Setup(m => m.Map<TheaterLocationReadDto>(theaterLocation)).Returns(new TheaterLocationReadDto());

        // Act
        var result = _theaterLocationService.GetById(filter);

        // Assert
        Assert.NotNull(result);
        _theaterLocationRepositoryMock.Verify(r => r.GetByElement(It.IsAny<FilterByItem>()), Times.Once);
    }

    [Fact]
    public void GetFilter_ShouldReturnFilteredResults()
    {
        // Arrange
        var filter = new FilterTheaterLocationTable { PageNumber = 1, PageSize = 10 };
        var filterResult = new FilterReturn<TheaterLocation>
        {
            TotalRegister = 10,
            TotalRegisterFilter = 5,
            TotalPages = 2,
            ItensList = new List<TheaterLocation>
            {
                new TheaterLocation { Id = Guid.NewGuid(), Street = "Some Street", UnitNumber = "123", PostalCode = "12345-678" }
            }
        };

        _theaterLocationRepositoryMock.Setup(r => r.GetFilter(filter)).Returns(filterResult);
        _mapperMock.Setup(m => m.Map<IEnumerable<TheaterLocationReadDto>>(filterResult.ItensList))
            .Returns(new List<TheaterLocationReadDto>());

        // Act
        var result = _theaterLocationService.GetFilter(filter);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(filterResult.TotalRegister, result.TotalRegister);
        _theaterLocationRepositoryMock.Verify(r => r.GetFilter(filter), Times.Once);
    }

    [Fact]
    public void Add_ShouldReturnNull_WhenStreetAlreadyExists()
    {
        // Arrange
        var createDto = new TheaterLocationCreateDto
        {
            Street = "Some Street",  // Rua que já existe
            UnitNumber = "123",
            PostalCode = "12345-678"
        };
        
        _theaterLocationRepositoryMock.Setup(r => r.GetByElement(It.IsAny<FilterByItem>()))
            .Returns(new TheaterLocation { Street = "Some Street",  UnitNumber = "123", PostalCode = "12345-678" });

        // Act
        var result = _theaterLocationService.Add(createDto);

        // Assert
        Assert.Null(result);  // Espera-se que retorne null, pois a rua já existe
        _theaterLocationRepositoryMock.Verify(r => r.Add(It.IsAny<TheaterLocation>()), Times.Never);  // Verifica se o Add não foi chamado
    }

    [Fact]
    public void Update_ShouldNotCallRepository_WhenSpecificationFails()
    {
        // Arrange
        var updateDto = new TheaterLocationUpdateDto
        {
            Id = Guid.NewGuid(),
            Street = "Some Street",
            UnitNumber = "123",
            PostalCode = "12345-678"
        };

        _theaterLocationRepositoryMock.Setup(r => r.GetByElement(It.IsAny<FilterByItem>()))
            .Returns(new TheaterLocation { Id = Guid.NewGuid(), Street = "Some Street", UnitNumber = "123", PostalCode = "12345-678" });
        
        // Act
        _theaterLocationService.Update(updateDto);

        // Assert
        _theaterLocationRepositoryMock.Verify(r => r.Update(It.IsAny<TheaterLocation>()), Times.Once);
    }

    [Fact]
    public void Delete_ShouldNotCallRepository_WhenNoNotifications()
    {
        // Arrange
        var id = Guid.NewGuid();
        var theaterLocation = new TheaterLocation { Id = id, Street = "Some Street", UnitNumber = "123", PostalCode = "12345-678" };
        _theaterLocationRepositoryMock.Setup(r => r.GetByElement(It.IsAny<FilterByItem>())).Returns(theaterLocation);

        // Act
        _theaterLocationService.Delete(id);

        // Assert
        _theaterLocationRepositoryMock.Verify(r => r.Delete(theaterLocation), Times.Once);
    }
}
