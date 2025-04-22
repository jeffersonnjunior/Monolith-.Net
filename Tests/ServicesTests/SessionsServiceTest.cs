using Application.Dtos;
using Application.Interfaces.IFactories;
using Application.Services;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Interfaces.IRepositories;
using Infrastructure.Notifications;
using Infrastructure.FiltersModel;
using Moq;

public class SessionsServiceTests
{
    private readonly Mock<ISessionsRepository> _sessionsRepositoryMock;
    private readonly Mock<ISessionsFactory> _sessionsFactoryMock;
    private readonly NotificationContext _notificationContext;
    private readonly SessionsService _sessionsService;

    public SessionsServiceTests()
    {
        _sessionsRepositoryMock = new Mock<ISessionsRepository>();
        _sessionsFactoryMock = new Mock<ISessionsFactory>();
        _notificationContext = new NotificationContext();
        _sessionsService = new SessionsService(
            _sessionsRepositoryMock.Object,
            _notificationContext,
            _sessionsFactoryMock.Object
        );
    }

    [Fact]
    public void GetById_ShouldReturnSessionReadDto_WhenSessionExists()
    {
        // Arrange
        var sessionId = Guid.NewGuid();
        var session = new Sessions
        {
            Id = sessionId,
            SessionTime = DateTime.Now,
            FilmId = Guid.NewGuid(),
            FilmAudioOption = FilmAudioOption.Dubbed,
            FilmFormat = FilmFormat.D2
        };
        var sessionReadDto = new SessionsReadDto
        {
            Id = sessionId,
            SessionTime = session.SessionTime,
            FilmId = session.FilmId
        };

        _sessionsRepositoryMock
            .Setup(repo => repo.GetByElement(It.IsAny<FilterByItem>()))
            .Returns(session);

        _sessionsFactoryMock
            .Setup(factory => factory.MapToSessionReadDto(session))
            .Returns(sessionReadDto);

        var filter = new FilterSessionsById { Id = sessionId };

        // Act
        var result = _sessionsService.GetById(filter);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(sessionId, result.Id);
        Assert.Equal(session.SessionTime, result.SessionTime);
    }

    [Fact]
    public void Add_ShouldReturnSessionUpdateDto_WhenInputIsValid()
    {
        // Arrange
        var sessionsCreateDto = new SessionsCreateDto
        {
            SessionTime = DateTime.Now,
            FilmId = Guid.NewGuid(),
            FilmAudioOption = FilmAudioOption.Dubbed,
            FilmFormat = FilmFormat.D2
        };
        var session = new Sessions
        {
            Id = Guid.NewGuid(),
            SessionTime = sessionsCreateDto.SessionTime,
            FilmId = sessionsCreateDto.FilmId,
            FilmAudioOption = sessionsCreateDto.FilmAudioOption,
            FilmFormat = sessionsCreateDto.FilmFormat
        };
        var sessionsUpdateDto = new SessionsUpdateDto
        {
            Id = session.Id,
            SessionTime = session.SessionTime,
            FilmId = session.FilmId
        };

        _sessionsRepositoryMock
            .Setup(repo => repo.ValidateInput(sessionsCreateDto, false, null))
            .Returns(true);

        _sessionsFactoryMock
            .Setup(factory => factory.MapToSession(sessionsCreateDto))
            .Returns(session);

        _sessionsRepositoryMock
            .Setup(repo => repo.Add(session))
            .Returns(session);

        _sessionsFactoryMock
            .Setup(factory => factory.MapToSessionUpdateDto(session))
            .Returns(sessionsUpdateDto);

        // Act
        var result = _sessionsService.Add(sessionsCreateDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(session.Id, result.Id);
        Assert.Equal(session.SessionTime, result.SessionTime);
    }

    [Fact]
    public void Delete_ShouldCallRepositoryDelete_WhenSessionExists()
    {
        // Arrange
        var sessionId = Guid.NewGuid();
        var session = new Sessions
        {
            Id = sessionId,
            SessionTime = DateTime.Now,
            FilmId = Guid.NewGuid()
        };

        _sessionsRepositoryMock
            .Setup(repo => repo.GetByElement(It.IsAny<FilterByItem>()))
            .Returns(session);

        // Act
        _sessionsService.Delete(sessionId);

        // Assert
        _sessionsRepositoryMock.Verify(repo => repo.Delete(session), Times.Once);
    }
}