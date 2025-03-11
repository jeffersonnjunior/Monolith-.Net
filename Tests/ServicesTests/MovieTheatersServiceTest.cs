using Application.Dtos;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces.IRepositories;
using Infrastructure.Utilities.FiltersModel;
using Moq;
using Xunit;

namespace ServicesTests;

public class MovieTheatersServiceTest
{
    private readonly Mock<IMovieTheatersRepository> _movieTheatersRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly MovieTheatersService _movieTheatersService;

    public MovieTheatersServiceTest()
    {
        _movieTheatersRepositoryMock = new Mock<IMovieTheatersRepository>();
        _mapperMock = new Mock<IMapper>();
        _movieTheatersService = new MovieTheatersService(
            _movieTheatersRepositoryMock.Object,
            _mapperMock.Object,
            null
        );
    }

    [Fact]
    public void GetById_ShouldReturnMovieTheater()
    {
        // Arrange
        var filter = new FilterMovieTheatersById { Id = Guid.NewGuid() };
        var movieTheater = new MovieTheaters { Id = filter.Id, Name = "Test Theater", TheaterLocationId = Guid.NewGuid() };
        var movieTheaterDto = new MovieTheatersReadDto { Id = filter.Id, Name = "Test Theater", TheaterLocationId = movieTheater.TheaterLocationId };

        _movieTheatersRepositoryMock.Setup(repo => repo.GetByElement(It.IsAny<FilterByItem>())).Returns(movieTheater);
        _mapperMock.Setup(mapper => mapper.Map<MovieTheatersReadDto>(movieTheater)).Returns(movieTheaterDto);

        // Act
        var result = _movieTheatersService.GetById(filter);

        // Assert
        Assert.Equal(movieTheaterDto, result);
    }

    [Fact]
    public void GetFilter_ShouldReturnFilteredMovieTheaters()
    {
        // Arrange
        var filter = new FilterMovieTheatersTable();
        var filterResult = new FilterReturn<MovieTheaters> { ItensList = new List<MovieTheaters>() };
        var filterResultDto = new FilterReturn<MovieTheatersReadDto> { ItensList = new List<MovieTheatersReadDto>() };

        _movieTheatersRepositoryMock.Setup(repo => repo.GetFilter(filter)).Returns(filterResult);
        _mapperMock.Setup(mapper => mapper.Map<IEnumerable<MovieTheatersReadDto>>(filterResult.ItensList)).Returns(filterResultDto.ItensList);

        // Act
        var result = _movieTheatersService.GetFilter(filter);

        // Assert
        Assert.Equal(filterResultDto.ItensList, result.ItensList);
    }

    [Fact]
    public void Add_ShouldReturnAddedMovieTheater()
    {
        // Arrange
        var createDto = new MovieTheatersCreateDto { Name = "New Theater", TheaterLocationId = Guid.NewGuid() };
        var movieTheater = new MovieTheaters { Id = Guid.NewGuid(), Name = createDto.Name, TheaterLocationId = createDto.TheaterLocationId };
        var updateDto = new MovieTheatersUpdateDto { Id = movieTheater.Id, Name = movieTheater.Name, TheaterLocationId = movieTheater.TheaterLocationId };

        _movieTheatersRepositoryMock.Setup(repo => repo.ValidateInput(createDto, false, null)).Returns(true);
        _mapperMock.Setup(mapper => mapper.Map<MovieTheaters>(createDto)).Returns(movieTheater);
        _movieTheatersRepositoryMock.Setup(repo => repo.Add(movieTheater)).Returns(movieTheater);
        _mapperMock.Setup(mapper => mapper.Map<MovieTheatersUpdateDto>(movieTheater)).Returns(updateDto);

        // Act
        var result = _mapperMock.Object.Map<MovieTheatersUpdateDto>(_movieTheatersRepositoryMock.Object.Add(movieTheater));

        // Assert
        Assert.NotNull(result);
        Assert.IsType<MovieTheatersUpdateDto>(result);
        Assert.Equal(movieTheater.Id, result.Id);
        Assert.Equal(createDto.Name, result.Name);
    }

    [Fact]
    public void Update_ShouldUpdateMovieTheater()
    {
        // Arrange
        var updateDto = new MovieTheatersUpdateDto { Id = Guid.NewGuid(), Name = "Updated Theater", TheaterLocationId = Guid.NewGuid() };
        var movieTheater = new MovieTheaters { Id = updateDto.Id, Name = updateDto.Name, TheaterLocationId = updateDto.TheaterLocationId };

        _movieTheatersRepositoryMock.Setup(repo => repo.GetByElement(It.IsAny<FilterByItem>())).Returns(movieTheater);
        _movieTheatersRepositoryMock.Setup(repo => repo.ValidateInput(updateDto, true, movieTheater)).Returns(true);
        _mapperMock.Setup(mapper => mapper.Map<MovieTheaters>(updateDto)).Returns(movieTheater);

        // Act
        _movieTheatersService.Update(updateDto);

        // Assert
        _movieTheatersRepositoryMock.Verify(repo => repo.Update(movieTheater), Times.Once);
    }

    [Fact]
    public void Delete_ShouldRemoveMovieTheater()
    {
        // Arrange
        var id = Guid.NewGuid();
        var movieTheater = new MovieTheaters { Id = id, Name = "Test Theater", TheaterLocationId = Guid.NewGuid() };

        _movieTheatersRepositoryMock.Setup(repo => repo.GetByElement(It.IsAny<FilterByItem>())).Returns(movieTheater);

        // Act
        _movieTheatersService.Delete(id);

        // Assert
        _movieTheatersRepositoryMock.Verify(repo => repo.Delete(movieTheater), Times.Once);
    }
}