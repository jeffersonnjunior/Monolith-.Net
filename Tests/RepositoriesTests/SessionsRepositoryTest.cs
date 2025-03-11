using Domain.Entities;
using Domain.Enums;
using Infrastructure.Interfaces.IRepositories;
using Infrastructure.Notifications;
using Infrastructure.Utilities.FiltersModel;
using Moq;
using Xunit;

namespace RepositoryTest;

public class SessionsRepositoryTest
{
    private readonly Mock<ISessionsRepository> _sessionsRepositoryMock;
    private readonly Mock<NotificationContext> _notificationContextMock;
    private readonly Mock<IFilmsRepository> _filmsRepositoryMock;

    public SessionsRepositoryTest()
    {
        _sessionsRepositoryMock = new Mock<ISessionsRepository>();
        _notificationContextMock = new Mock<NotificationContext>();
        _filmsRepositoryMock = new Mock<IFilmsRepository>();
    }

    [Fact]
    public void GetByElement_ShouldReturnSession()
    {
        // Arrange
        var filterByItem = new FilterByItem { Field = "Id", Value = Guid.NewGuid(), Key = "Equal" };
        var session = new Sessions { SessionTime = DateTime.Now, FilmId = Guid.NewGuid(), FilmAudioOption = FilmAudioOption.Dubbed, FilmFormat = FilmFormat.Vip };

        _sessionsRepositoryMock.Setup(repo => repo.GetByElement(It.IsAny<FilterByItem>())).Returns(session);

        // Act
        var result = _sessionsRepositoryMock.Object.GetByElement(filterByItem);

        // Assert
        Assert.Equal(session, result);
    }

    [Fact]
    public void GetFilter_ShouldReturnFilterReturnOfSessions()
    {
        // Arrange
        var filter = new FilterSessionsTable();
        var filterResult = new FilterReturn<Sessions> { ItensList = new List<Sessions>() };

        _sessionsRepositoryMock.Setup(repo => repo.GetFilter(filter)).Returns(filterResult);

        // Act
        var result = _sessionsRepositoryMock.Object.GetFilter(filter);

        // Assert
        Assert.Equal(filterResult.ItensList, result.ItensList);
    }
}