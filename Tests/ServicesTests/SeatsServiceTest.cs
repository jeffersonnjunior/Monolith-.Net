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

public class SeatsServiceTest
{
    private readonly Mock<ISeatsRepository> _seatsRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<NotificationContext> _notificationContextMock;
    private readonly SeatsService _seatsService;

    public SeatsServiceTest()
    {
        _seatsRepositoryMock = new Mock<ISeatsRepository>();
        _mapperMock = new Mock<IMapper>();
        _notificationContextMock = new Mock<NotificationContext>();
        _seatsService = new SeatsService(
            _seatsRepositoryMock.Object,
            _mapperMock.Object,
            _notificationContextMock.Object
        );
    }

    [Fact]
    public void GetById_ShouldReturnNull_WhenSpecificationFails()
    {
        // Arrange
        var filter = new FilterSeatsById
        {
            Id = Guid.NewGuid(),
            Includes = new List<string> { "Screen", "Tickets" }.ToArray() 
        };

        // Act
        var result = _seatsService.GetById(filter);

        // Assert
        Assert.Null(result);
        _seatsRepositoryMock.Verify(r => r.GetByElement(It.IsAny<FilterByItem>()), Times.Never);
    }

    [Fact]
    public void GetFilter_ShouldReturnFilteredResults()
    {
        // Arrange
        var filter = new FilterSeatsTable
        {
            PageNumber = 1,
            PageSize = 10
        };

        var filterResult = new FilterReturn<Seats>
        {
            TotalRegister = 10,
            TotalRegisterFilter = 5,
            TotalPages = 2,
            ItensList = new List<Seats>
            {
                new Seats
                {
                    Id = Guid.NewGuid(),
                    SeatNumber = 1,
                    RowLetter = "A",
                    ScreenId = Guid.NewGuid(),
                    Tickets = new List<Tickets>(),
                    Screen = new Screens
                    {
                        ScreenNumber = "Screen 1",
                        MovieTheaterId = Guid.NewGuid() 
                    }
                }
            }
        };


        _seatsRepositoryMock.Setup(r => r.GetFilter(filter)).Returns(filterResult);
        _mapperMock.Setup(m => m.Map<IEnumerable<SeatsReadDto>>(filterResult.ItensList))
            .Returns(new List<SeatsReadDto>());

        // Act
        var result = _seatsService.GetFilter(filter);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(filterResult.TotalRegister, result.TotalRegister);
        _seatsRepositoryMock.Verify(r => r.GetFilter(filter), Times.Once);
    }

    [Fact]
    public void Add_ShouldReturnNull_WhenSpecificationFails()
    {
        // Arrange
        var createDto = new SeatsCreateDto
        {
            SeatNumber = 1,
            RowLetter = "B",
            ScreenId = Guid.NewGuid()
        };

        _seatsRepositoryMock.Setup(r => r.ValidateInput(createDto, false, null)).Returns(false);

        // Act
        var result = _seatsService.Add(createDto);

        // Assert
        Assert.Null(result);
        _seatsRepositoryMock.Verify(r => r.Add(It.IsAny<Seats>()), Times.Never);
    }

    [Fact]
    public void Update_ShouldNotCallRepository_WhenSpecificationFails()
    {
        // Arrange
        var updateDto = new SeatsUpdateDto
        {
            Id = Guid.NewGuid(),
            SeatNumber = 2,
            RowLetter = "C",
            ScreenId = Guid.NewGuid()
        };

        _seatsRepositoryMock.Setup(r => r.ValidateInput(updateDto, true, It.IsAny<Seats>())).Returns(false);

        // Act
        _seatsService.Update(updateDto);

        // Assert
        _seatsRepositoryMock.Verify(r => r.Update(It.IsAny<Seats>()), Times.Never);
    }

    [Fact]
    public void Delete_ShouldNotCallRepository_WhenSeatNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();

        _seatsRepositoryMock.Setup(r => r.GetByElement(It.IsAny<FilterByItem>())).Returns((Seats)null);

        // Act
        _seatsService.Delete(id);

        // Assert
        _seatsRepositoryMock.Verify(r => r.Delete(It.IsAny<Seats>()), Times.Never);
    }
}
