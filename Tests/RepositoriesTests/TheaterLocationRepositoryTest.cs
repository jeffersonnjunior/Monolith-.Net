using Domain.Entities;
using Infrastructure.Interfaces.IRepositories;
using Infrastructure.Notifications;
using Infrastructure.FiltersModel;
using Moq;

namespace RepositoryTest;

public class TheaterLocationRepositoryTest
{
    private readonly Mock<ITheaterLocationRepository> _theaterLocationRepositoryMock;
    private readonly Mock<NotificationContext> _notificationContextMock;

    public TheaterLocationRepositoryTest()
    {
        _theaterLocationRepositoryMock = new Mock<ITheaterLocationRepository>();
        _notificationContextMock = new Mock<NotificationContext>();
    }

    [Fact]
    public void GetByElement_ShouldReturnTheaterLocation()
    {
        // Arrange
        var filterByItem = new FilterByItem { Field = "Id", Value = Guid.NewGuid(), Key = "Equal" };
        var theaterLocation = new TheaterLocation { Id = (Guid)filterByItem.Value, Street = "Some Street", UnitNumber = "123", PostalCode = "12345-678" };

        _theaterLocationRepositoryMock.Setup(repo => repo.GetByElement(It.IsAny<FilterByItem>())).Returns(theaterLocation);

        // Act
        var result = _theaterLocationRepositoryMock.Object.GetByElement(filterByItem);

        // Assert
        Assert.Equal(theaterLocation, result);
    }

    [Fact]
    public void GetFilter_ShouldReturnFilterReturnOfTheaterLocations()
    {
        // Arrange
        var filter = new FilterTheaterLocationTable();
        var filterResult = new FilterReturn<TheaterLocation> { ItensList = new List<TheaterLocation>() };

        _theaterLocationRepositoryMock.Setup(repo => repo.GetFilter(filter)).Returns(filterResult);

        // Act
        var result = _theaterLocationRepositoryMock.Object.GetFilter(filter);

        // Assert
        Assert.Equal(filterResult.ItensList, result.ItensList);
    }
}