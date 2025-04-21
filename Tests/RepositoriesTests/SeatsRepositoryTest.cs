using Domain.Entities;
using Infrastructure.Interfaces.IRepositories;
using Infrastructure.Notifications;
using Infrastructure.FiltersModel;
using Moq;

namespace RepositoryTest;

public class SeatsRepositoryTest
{
    private readonly Mock<ISeatsRepository> _seatsRepositoryMock;
    private readonly Mock<NotificationContext> _notificationContextMock;
    private readonly Mock<IScreensRepository> _screensRepositoryMock;

    public SeatsRepositoryTest()
    {
        _seatsRepositoryMock = new Mock<ISeatsRepository>();
        _notificationContextMock = new Mock<NotificationContext>();
        _screensRepositoryMock = new Mock<IScreensRepository>();
    }

    [Fact]
    public void GetByElement_ShouldReturnSeat()
    {
        // Arrange
        var filterByItem = new FilterByItem { Field = "Id", Value = Guid.NewGuid(), Key = "Equal" };
        var seat = new Seats { SeatNumber = 1, RowLetter = "A", ScreenId = Guid.NewGuid() };

        _seatsRepositoryMock.Setup(repo => repo.GetByElement(It.IsAny<FilterByItem>())).Returns(seat);

        // Act
        var result = _seatsRepositoryMock.Object.GetByElement(filterByItem);

        // Assert
        Assert.Equal(seat, result);
    }

    [Fact]
    public void GetFilter_ShouldReturnFilterReturnOfSeats()
    {
        // Arrange
        var filter = new FilterSeatsTable();
        var filterResult = new FilterReturn<Seats> { ItensList = new List<Seats>() };

        _seatsRepositoryMock.Setup(repo => repo.GetFilter(filter)).Returns(filterResult);

        // Act
        var result = _seatsRepositoryMock.Object.GetFilter(filter);

        // Assert
        Assert.Equal(filterResult.ItensList, result.ItensList);
    }
}