using Application.Dtos;
using Application.Services;
using Application.Specification;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Interfaces.IRepositories;
using Infrastructure.Notifications;
using Infrastructure.Utilities.FiltersModel;
using Moq;
using Xunit;

namespace ServicesTest;

public class SessionsServiceTest
{
    private readonly Mock<ISessionsRepository> _sessionsRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<NotificationContext> _notificationContextMock;
    private readonly SessionsSpecification _sessionsSpecification;
    private readonly SessionsService _sessionsService;

    public SessionsServiceTest()
    {
        _sessionsRepositoryMock = new Mock<ISessionsRepository>();
        _mapperMock = new Mock<IMapper>();
        _notificationContextMock = new Mock<NotificationContext>();
        _sessionsSpecification = new SessionsSpecification(_notificationContextMock.Object);
        _sessionsService = new SessionsService(
            _sessionsRepositoryMock.Object,
            _mapperMock.Object,
            _notificationContextMock.Object
        );
    }

    [Fact]
    public void GetById_ShouldReturnNull_WhenSpecificationFails()
    {
        // Arrange
        var filter = new FilterSessionsById
        {
            Id = Guid.NewGuid(),
            Includes = new List<string> { "Film", "Tickets" }.ToArray()
        };

        // Act
        var result = _sessionsService.GetById(filter);

        // Assert
        Assert.Null(result);
        _sessionsRepositoryMock.Verify(r => r.GetByElement(It.IsAny<FilterByItem>()), Times.Never);
    }

    [Fact]
    public void GetFilter_ShouldReturnFilteredResults()
    {
        // Arrange
        var filter = new FilterSessionsTable
        {
            PageNumber = 1,
            PageSize = 10
        };

        var filterResult = new FilterReturn<Sessions>
        {
            TotalRegister = 10,
            TotalRegisterFilter = 5,
            TotalPages = 2,
            ItensList = new List<Sessions>
            {
                new Sessions
                {
                    Id = Guid.NewGuid(),
                    SessionTime = DateTime.Now,
                    FilmId = Guid.NewGuid(),
                    FilmAudioOption = FilmAudioOption.Dubbed,
                    FilmFormat = FilmFormat.Vip
                }
            }
        };

        _sessionsRepositoryMock.Setup(r => r.GetFilter(filter)).Returns(filterResult);
        _mapperMock.Setup(m => m.Map<IEnumerable<SessionsReadDto>>(filterResult.ItensList))
            .Returns(new List<SessionsReadDto>());

        // Act
        var result = _sessionsService.GetFilter(filter);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(filterResult.TotalRegister, result.TotalRegister);
        _sessionsRepositoryMock.Verify(r => r.GetFilter(filter), Times.Once);
    }

    [Fact]
    public void Add_ShouldReturnNull_WhenSpecificationFails()
    {
        // Arrange
        var createDto = new SessionsCreateDto
        {
            SessionTime = DateTime.Now,
            FilmId = Guid.NewGuid(),
            FilmAudioOption = FilmAudioOption.Dubbed,
            FilmFormat = FilmFormat.Vip
        };

        _sessionsRepositoryMock.Setup(r => r.ValidateInput(createDto, false, null)).Returns(false);

        // Act
        var result = _sessionsService.Add(createDto);

        // Assert
        Assert.Null(result);
        _sessionsRepositoryMock.Verify(r => r.Add(It.IsAny<Sessions>()), Times.Never);
    }

    [Fact]
    public void Update_ShouldNotCallRepository_WhenSpecificationFails()
    {
        // Arrange
        var updateDto = new SessionsUpdateDto
        {
            Id = Guid.NewGuid(),
            SessionTime = DateTime.Now,
            FilmId = Guid.NewGuid(),
            FilmAudioOption = FilmAudioOption.Dubbed,
            FilmFormat = FilmFormat.D3
        };

        _sessionsRepositoryMock.Setup(r => r.ValidateInput(updateDto, false, It.IsAny<Sessions>())).Returns(false);

        // Act
        _sessionsService.Update(updateDto);

        // Assert
        _sessionsRepositoryMock.Verify(r => r.Update(It.IsAny<Sessions>()), Times.Never);
    }

    [Fact]
    public void Delete_ShouldNotCallRepository_WhenSessionNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();

        _sessionsRepositoryMock.Setup(r => r.GetByElement(It.IsAny<FilterByItem>())).Returns((Sessions)null);

        // Act
        _sessionsService.Delete(id);

        // Assert
        _sessionsRepositoryMock.Verify(r => r.Delete(It.IsAny<Sessions>()), Times.Never);
    }
}
