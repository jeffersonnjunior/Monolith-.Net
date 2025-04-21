using Application.Dtos;
using Application.Interfaces.IFactory;
using Application.Services;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Interfaces.IRepositories;
using Infrastructure.Notifications;
using Infrastructure.FiltersModel;
using Moq;

public class FilmsServiceTest
{
    private readonly Mock<IFilmsRepository> _filmsRepositoryMock;
    private readonly Mock<IFilmeFactory> _filmeFactoryMock;
    private readonly Mock<NotificationContext> _notificationContextMock;
    private readonly FilmsService _filmsService;

    public FilmsServiceTest()
    {
        _filmsRepositoryMock = new Mock<IFilmsRepository>();
        _filmeFactoryMock = new Mock<IFilmeFactory>();
        _notificationContextMock = new Mock<NotificationContext>();
        _filmsService = new FilmsService(
            _filmsRepositoryMock.Object,
            _notificationContextMock.Object,
            _filmeFactoryMock.Object
        );
    }

    [Fact]
    public void GetById_ShouldReturnFilmsReadDto_WhenSpecificationIsSatisfied()
    {
        // Arrange
        var filter = new FilterFilmsById
        {
            Id = Guid.NewGuid(),
            Includes = new[] { "Sessions" }
        };
        var film = new Films
        {
            Id = filter.Id,
            Name = "Film 1",
            Duration = 120,
            AgeRange = 16,
            FilmGenres = FilmGenres.Action,
            Sessions = new List<Sessions>()
        };
        var filmsReadDto = new FilmsReadDto
        {
            Name = "Film 1",
            Duration = 120,
            AgeRange = 16
        };

        _filmsRepositoryMock
            .Setup(repo => repo.GetByElement(It.IsAny<FilterByItem>()))
            .Returns(film);
        _filmeFactoryMock
            .Setup(factory => factory.MapToFilmeReadDto(film))
            .Returns(filmsReadDto);

        // Act
        var result = _filmsService.GetById(filter);

        // Assert
        Assert.Equal(filmsReadDto, result);
    }

    [Fact]
    public void GetFilter_ShouldReturnFilteredFilms()
    {
        // Arrange
        var filter = new FilterFilmsTable();
        var filterResult = new FilterReturn<Films>
        {
            TotalRegister = 10,
            TotalRegisterFilter = 5,
            TotalPages = 2,
            ItensList = new List<Films>
            {
                new Films
                {
                    Id = Guid.NewGuid(),
                    Name = "Film 1",
                    Duration = 120,
                    AgeRange = 16,
                    FilmGenres = FilmGenres.Action,
                    Sessions = new List<Sessions>()
                }
            }
        };

        _filmsRepositoryMock
            .Setup(repo => repo.GetFilter(filter))
            .Returns(filterResult);
        _filmeFactoryMock
            .Setup(factory => factory.MapToFilmeReadDto(It.IsAny<Films>()))
            .Returns(new FilmsReadDto
            {
                Name = "Film 1",
                Duration = 120,
                AgeRange = 16
            });

        // Act
        var result = _filmsService.GetFilter(filter);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(filterResult.TotalRegister, result.TotalRegister);
        Assert.Equal(filterResult.TotalRegisterFilter, result.TotalRegisterFilter);
    }

    [Fact]
    public void Add_ShouldReturnFilmsUpdateDto_WhenValidInput()
    {
        // Arrange
        var filmsCreateDto = new FilmsCreateDto
        {
            Name = "Film 1",
            Duration = 120,
            AgeRange = 16,
            FilmGenres = FilmGenres.Action
        };
        var film = new Films
        {
            Id = Guid.NewGuid(),
            Name = "Film 1",
            Duration = 120,
            AgeRange = 16,
            FilmGenres = FilmGenres.Action,
            Sessions = new List<Sessions>()
        };
        var filmsUpdateDto = new FilmsUpdateDto
        {
            Name = "Film 1",
            Duration = 120,
            AgeRange = 16
        };

        _filmsRepositoryMock
            .Setup(repo => repo.ValidateInput(filmsCreateDto, false, null))
            .Returns(true);
        _filmeFactoryMock
            .Setup(factory => factory.MapToFilme(filmsCreateDto))
            .Returns(film);
        _filmsRepositoryMock
            .Setup(repo => repo.Add(film))
            .Returns(film);
        _filmeFactoryMock
            .Setup(factory => factory.MapToFilmeUpdateDto(film))
            .Returns(filmsUpdateDto);

        // Act
        var result = _filmsService.Add(filmsCreateDto);

        // Assert
        Assert.Equal(filmsUpdateDto.Name, result.Name);
    }

    [Fact]
    public void Delete_ShouldCallRepositoryDelete_WhenFilmExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var film = new Films
        {
            Id = id,
            Name = "Film 1",
            Duration = 120,
            AgeRange = 16,
            FilmGenres = FilmGenres.Action,
            Sessions = new List<Sessions>()
        };

        _filmsRepositoryMock
            .Setup(repo => repo.GetByElement(It.IsAny<FilterByItem>()))
            .Returns(film);

        // Act
        _filmsService.Delete(id);

        // Assert
        _filmsRepositoryMock.Verify(repo => repo.Delete(film), Times.Once);
    }
}