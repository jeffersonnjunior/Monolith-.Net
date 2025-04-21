using Domain.Entities;
using Infrastructure.Interfaces.IRepositories;
using Infrastructure.Notifications;
using Infrastructure.FiltersModel;
using Moq;

namespace RepositoryTest;

public class ScreensRepositoryTest
{
    private readonly Mock<IScreensRepository> _screensRepositoryMock;
    private readonly Mock<NotificationContext> _notificationContextMock;

    public ScreensRepositoryTest()
    {
        _screensRepositoryMock = new Mock<IScreensRepository>();
        _notificationContextMock = new Mock<NotificationContext>();
    }

    [Fact]
    public void GetByElement_ShouldReturnScreen()
    {
        // Arrange
        var filterByItem = new FilterByItem { Field = "Id", Value = Guid.NewGuid(), Key = "Equal" };
        var screen = new Screens { ScreenNumber = "Screen 1", MovieTheaterId = Guid.NewGuid() };

        _screensRepositoryMock.Setup(repo => repo.GetByElement(It.IsAny<FilterByItem>())).Returns(screen);

        // Act
        var result = _screensRepositoryMock.Object.GetByElement(filterByItem);

        // Assert
        Assert.Equal(screen, result);
    }
    
    [Fact]
    public void GetFilter_ShouldReturnFilterReturnOfScreens()
    {
        // Arrange
        var filter = new FilterScreensTable();
        var filterResult = new FilterReturn<Screens> { ItensList = new List<Screens>() };

        _screensRepositoryMock.Setup(repo => repo.GetFilter(filter)).Returns(filterResult);

        // Act
        var result = _screensRepositoryMock.Object.GetFilter(filter);

        // Assert
        Assert.Equal(filterResult.ItensList, result.ItensList);
    }
}