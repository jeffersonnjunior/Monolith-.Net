using Application.Dtos;
using Application.Services;
using Application.Specification;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces.IRepositories;
using Infrastructure.Notifications;
using Infrastructure.Utilities.FiltersModel;
using Moq;

namespace ServicesTests;

public class ScreensServiceTest
{
    private readonly Mock<IScreensRepository> _screensRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<NotificationContext> _notificationContextMock;
    private readonly ScreensSpecification _screensSpecification;
    private readonly ScreensService _screensService;

    public ScreensServiceTest()
    {
        _screensRepositoryMock = new Mock<IScreensRepository>();
        _mapperMock = new Mock<IMapper>();
        _notificationContextMock = new Mock<NotificationContext>();
        _screensSpecification = new ScreensSpecification(_notificationContextMock.Object);
        _screensService = new ScreensService(
            _screensRepositoryMock.Object,
            _mapperMock.Object,
            _notificationContextMock.Object
        );
    }

    [Fact]
    public void GetById_ShouldReturnNull_WhenSpecificationFails()
    {
        // Arrange
        var filter = new FilterScreensById
        {
            Id = Guid.NewGuid(),
            Includes = new List<string> { "Theater", "Seats" }.ToArray() // Conversão para array
        };

        // Act
        var result = _screensService.GetById(filter);

        // Assert
        Assert.Null(result);
        _screensRepositoryMock.Verify(r => r.GetByElement(It.IsAny<FilterByItem>()), Times.Never);
    }

    [Fact]
    public void GetFilter_ShouldReturnFilteredResults()
    {
        // Arrange
        var filter = new FilterScreensTable
        {
            PageNumber = 1,
            PageSize = 10
        };

        var filterResult = new FilterReturn<Screens>
        {
            TotalRegister = 10,
            TotalRegisterFilter = 5,
            TotalPages = 2,
            ItensList = new List<Screens>
            {
                new Screens
                {
                    Id = Guid.NewGuid(),
                    ScreenNumber = "Screen 1",
                    SeatingCapacity = 100,
                    MovieTheaterId = Guid.NewGuid() // Propriedade obrigatória inicializada
                }
            }
        };

        _screensRepositoryMock.Setup(r => r.GetFilter(filter)).Returns(filterResult);
        _mapperMock.Setup(m => m.Map<IEnumerable<ScreensReadDto>>(filterResult.ItensList))
            .Returns(new List<ScreensReadDto>());

        // Act
        var result = _screensService.GetFilter(filter);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(filterResult.TotalRegister, result.TotalRegister);
        _screensRepositoryMock.Verify(r => r.GetFilter(filter), Times.Once);
    }


    [Fact]
    public void Add_ShouldReturnNull_WhenSpecificationFails()
    {
        // Arrange
        var createDto = new ScreensCreateDto
        {
            ScreenNumber = "Screen 1",
            SeatingCapacity = 100,
            MovieTheaterId = Guid.NewGuid()
        };

        _screensRepositoryMock.Setup(r => r.ValidateInput(createDto, false, null)).Returns(false);

        // Act
        var result = _screensService.Add(createDto);

        // Assert
        Assert.Null(result);
        _screensRepositoryMock.Verify(r => r.Add(It.IsAny<Screens>()), Times.Never);
    }

    [Fact]
    public void Update_ShouldNotCallRepository_WhenSpecificationFails()
    {
        // Arrange
        var updateDto = new ScreensUpdateDto
        {
            Id = Guid.NewGuid(),
            ScreenNumber = "Screen 2",
            SeatingCapacity = 120,
            MovieTheaterId = Guid.NewGuid()
        };

        _screensRepositoryMock.Setup(r => r.ValidateInput(updateDto, true, It.IsAny<Screens>())).Returns(false);

        // Act
        _screensService.Update(updateDto);

        // Assert
        _screensRepositoryMock.Verify(r => r.Update(It.IsAny<Screens>()), Times.Never);
    }

    [Fact]
    public void Delete_ShouldNotCallRepository_WhenScreenNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();

        _screensRepositoryMock.Setup(r => r.GetByElement(It.IsAny<FilterByItem>())).Returns((Screens)null);

        // Act
        _screensService.Delete(id);

        // Assert
        _screensRepositoryMock.Verify(r => r.Delete(It.IsAny<Screens>()), Times.Never);
    }
}
