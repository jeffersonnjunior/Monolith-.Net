using Application.Dtos;
using Application.Interfaces.IFactory;
using Application.Services;
using Domain.Entities;
using Infrastructure.Interfaces.IRepositories;
using Infrastructure.Notifications;
using Infrastructure.FiltersModel;
using Moq;

public class SeatsServiceTests
{
    private readonly Mock<ISeatsRepository> _seatsRepositoryMock;
    private readonly Mock<ISeatsFactory> _seatsFactoryMock;
    private readonly NotificationContext _notificationContext;
    private readonly SeatsService _seatsService;

    public SeatsServiceTests()
    {
        _seatsRepositoryMock = new Mock<ISeatsRepository>();
        _seatsFactoryMock = new Mock<ISeatsFactory>();
        _notificationContext = new NotificationContext();
        _seatsService = new SeatsService(
            _seatsRepositoryMock.Object,
            _notificationContext,
            _seatsFactoryMock.Object
        );
    }

    [Fact]
    public void GetById_ShouldReturnSeatReadDto_WhenSeatExists()
    {
        // Arrange
        var seatId = Guid.NewGuid();
        var seat = new Seats { Id = seatId, SeatNumber = 1, RowLetter = "A", ScreenId = Guid.NewGuid() };
        var seatReadDto = new SeatsReadDto { Id = seatId, SeatNumber = 1, RowLetter = "A" };

        _seatsRepositoryMock
            .Setup(repo => repo.GetByElement(It.IsAny<FilterByItem>()))
            .Returns(seat);

        _seatsFactoryMock
            .Setup(factory => factory.MapToSeatReadDto(seat))
            .Returns(seatReadDto);

        var filter = new FilterSeatsById { Id = seatId };

        // Act
        var result = _seatsService.GetById(filter);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(seatId, result.Id);
        Assert.Equal("A", result.RowLetter);
    }

    [Fact]
    public void Add_ShouldReturnSeatsUpdateDto_WhenInputIsValid()
    {
        // Arrange
        var seatsCreateDto = new SeatsCreateDto { SeatNumber = 1, RowLetter = "A", ScreenId = Guid.NewGuid() };
        var seat = new Seats { Id = Guid.NewGuid(), SeatNumber = 1, RowLetter = "A", ScreenId = seatsCreateDto.ScreenId };
        var seatsUpdateDto = new SeatsUpdateDto { Id = seat.Id, SeatNumber = 1, RowLetter = "A" };

        _seatsRepositoryMock
            .Setup(repo => repo.ValidateInput(seatsCreateDto, false, null))
            .Returns(true);

        _seatsFactoryMock
            .Setup(factory => factory.MapToSeat(seatsCreateDto))
            .Returns(seat);

        _seatsRepositoryMock
            .Setup(repo => repo.Add(seat))
            .Returns(seat);

        _seatsFactoryMock
            .Setup(factory => factory.MapToSeatUpdateDto(seat))
            .Returns(seatsUpdateDto);

        // Act
        var result = _seatsService.Add(seatsCreateDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(seat.Id, result.Id);
        Assert.Equal("A", result.RowLetter);
    }

    [Fact]
    public void Delete_ShouldCallRepositoryDelete_WhenSeatExists()
    {
        // Arrange
        var seatId = Guid.NewGuid();
        var seat = new Seats { Id = seatId, SeatNumber = 1, RowLetter = "A", ScreenId = Guid.NewGuid() };

        _seatsRepositoryMock
            .Setup(repo => repo.GetByElement(It.IsAny<FilterByItem>()))
            .Returns(seat);

        // Act
        _seatsService.Delete(seatId);

        // Assert
        _seatsRepositoryMock.Verify(repo => repo.Delete(seat), Times.Once);
    }
}