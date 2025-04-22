using Application.Dtos;
using Application.Interfaces.IFactories;
using Application.Services;
using Domain.Entities;
using Infrastructure.Interfaces.IRepositories;
using Infrastructure.Notifications;
using Infrastructure.FiltersModel;
using Moq;

public class MovieTheatersServiceTest
{
    private readonly Mock<IMovieTheatersRepository> _movieTheatersRepositoryMock;
    private readonly Mock<IMovieTheatersFactory> _movieTheatersFactoryMock;
    private readonly Mock<NotificationContext> _notificationContextMock;
    private readonly MovieTheatersService _movieTheatersService;

    public MovieTheatersServiceTest()
    {
        _movieTheatersRepositoryMock = new Mock<IMovieTheatersRepository>();
        _movieTheatersFactoryMock = new Mock<IMovieTheatersFactory>();
        _notificationContextMock = new Mock<NotificationContext>();
        _movieTheatersService = new MovieTheatersService(
            _movieTheatersRepositoryMock.Object,
            _notificationContextMock.Object,
            _movieTheatersFactoryMock.Object
        );
    }

    [Fact]
    public void GetById_ShouldReturnMovieTheatersReadDto_WhenMovieTheaterExists()
    {
        // Arrange
        var filter = new FilterMovieTheatersById
        {
            Id = Guid.NewGuid(),
            Includes = new[] { "Screens" }
        };
        var movieTheater = new MovieTheaters
        {
            Id = filter.Id,
            Name = "Theater 1",
            TheaterLocationId = Guid.NewGuid(),
            Screens = new List<Screens>()
        };
        var movieTheatersReadDto = new MovieTheatersReadDto
        {
            Name = "Theater 1"
        };

        _movieTheatersRepositoryMock
            .Setup(repo => repo.GetByElement(It.IsAny<FilterByItem>()))
            .Returns(movieTheater);
        _movieTheatersFactoryMock
            .Setup(factory => factory.MapToMovieTheaterReadDto(movieTheater))
            .Returns(movieTheatersReadDto);

        // Act
        var result = _movieTheatersService.GetById(filter);

        // Assert
        Assert.Equal(movieTheatersReadDto, result);
    }

    [Fact]
    public void GetFilter_ShouldReturnFilteredMovieTheaters()
    {
        // Arrange
        var filter = new FilterMovieTheatersTable();
        var filterResult = new FilterReturn<MovieTheaters>
        {
            TotalRegister = 10,
            TotalRegisterFilter = 5,
            TotalPages = 2,
            ItensList = new List<MovieTheaters>
            {
                new MovieTheaters
                {
                    Id = Guid.NewGuid(),
                    Name = "Theater 1",
                    TheaterLocationId = Guid.NewGuid(),
                    Screens = new List<Screens>()
                }
            }
        };

        _movieTheatersRepositoryMock
            .Setup(repo => repo.GetFilter(filter))
            .Returns(filterResult);
        _movieTheatersFactoryMock
            .Setup(factory => factory.MapToMovieTheaterReadDto(It.IsAny<MovieTheaters>()))
            .Returns(new MovieTheatersReadDto
            {
                Name = "Theater 1"
            });

        // Act
        var result = _movieTheatersService.GetFilter(filter);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(filterResult.TotalRegister, result.TotalRegister);
        Assert.Equal(filterResult.TotalRegisterFilter, result.TotalRegisterFilter);
    }

    [Fact]
    public void Add_ShouldReturnMovieTheatersUpdateDto_WhenValidInput()
    {
        // Arrange
        var movieTheatersCreateDto = new MovieTheatersCreateDto
        {
            Name = "Theater 1",
            TheaterLocationId = Guid.NewGuid()
        };
        var movieTheater = new MovieTheaters
        {
            Id = Guid.NewGuid(),
            Name = "Theater 1",
            TheaterLocationId = movieTheatersCreateDto.TheaterLocationId,
            Screens = new List<Screens>()
        };
        var movieTheatersUpdateDto = new MovieTheatersUpdateDto
        {
            Id = Guid.NewGuid(),
            Name = "Theater 1",
            TheaterLocationId = movieTheatersCreateDto.TheaterLocationId,
            
        };

        _movieTheatersRepositoryMock
            .Setup(repo => repo.ValidateInput(movieTheatersCreateDto, false, null))
            .Returns(true);
        _movieTheatersFactoryMock
            .Setup(factory => factory.MapToMovieTheater(movieTheatersCreateDto))
            .Returns(movieTheater);
        _movieTheatersRepositoryMock
            .Setup(repo => repo.Add(movieTheater))
            .Returns(movieTheater);
        _movieTheatersFactoryMock
            .Setup(factory => factory.MapToMovieTheaterUpdateDto(movieTheater))
            .Returns(movieTheatersUpdateDto);

        // Act
        var result = _movieTheatersService.Add(movieTheatersCreateDto);

        // Assert
        Assert.Equal(movieTheatersUpdateDto.Name, result.Name);
    }

    [Fact]
    public void Update_ShouldCallRepositoryUpdate_WhenValidInput()
    {
        // Arrange
        var movieTheatersUpdateDto = new MovieTheatersUpdateDto
        {
            Id = Guid.NewGuid(),
            Name = "Theater 1",
            TheaterLocationId = Guid.NewGuid(),
        };
        var movieTheater = new MovieTheaters
        {
            Id = movieTheatersUpdateDto.Id,
            Name = "Theater 1",
            TheaterLocationId = Guid.NewGuid(),
            Screens = new List<Screens>()
        };

        _movieTheatersRepositoryMock
            .Setup(repo => repo.GetByElement(It.IsAny<FilterByItem>()))
            .Returns(movieTheater);
        _movieTheatersRepositoryMock
            .Setup(repo => repo.ValidateInput(movieTheatersUpdateDto, true, movieTheater))
            .Returns(true);
        _movieTheatersFactoryMock
            .Setup(factory => factory.MapToMovieTheaterFromUpdateDto(movieTheatersUpdateDto))
            .Returns(movieTheater);

        // Act
        _movieTheatersService.Update(movieTheatersUpdateDto);

        // Assert
        _movieTheatersRepositoryMock.Verify(repo => repo.Update(movieTheater), Times.Once);
    }

    [Fact]
    public void Delete_ShouldCallRepositoryDelete_WhenMovieTheaterExists()
    {
        // Arrange
        var id = Guid.NewGuid();
        var movieTheater = new MovieTheaters
        {
            Id = id,
            Name = "Theater 1",
            TheaterLocationId = Guid.NewGuid(),
            Screens = new List<Screens>()
        };

        _movieTheatersRepositoryMock
            .Setup(repo => repo.GetByElement(It.IsAny<FilterByItem>()))
            .Returns(movieTheater);

        // Act
        _movieTheatersService.Delete(id);

        // Assert
        _movieTheatersRepositoryMock.Verify(repo => repo.Delete(movieTheater), Times.Once);
    }

    [Fact]
    public void Delete_ShouldNotCallRepositoryDelete_WhenMovieTheaterDoesNotExist()
    {
        // Arrange
        var id = Guid.NewGuid();

        _movieTheatersRepositoryMock
            .Setup(repo => repo.GetByElement(It.IsAny<FilterByItem>()))
            .Returns((MovieTheaters)null);

        // Act
        _movieTheatersService.Delete(id);

        // Assert
        _movieTheatersRepositoryMock.Verify(repo => repo.Delete(It.IsAny<MovieTheaters>()), Times.Never);
    }
}