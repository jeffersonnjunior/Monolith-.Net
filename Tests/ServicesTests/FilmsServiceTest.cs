using Application.Dtos;
using Application.Interfaces.IServices;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces.IRepositories;
using Infrastructure.Notifications;
using Infrastructure.Utilities.FiltersModel;
using Moq;
using System;
using System.Collections.Generic;
using Domain.Enums;
using Xunit;

namespace ServicesTests;

public class FilmsServiceTests
{
    private readonly Mock<IFilmsRepository> _filmsRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly NotificationContext _notifierContext;
    private readonly FilmsService _filmsService;

    public FilmsServiceTests()
    {
        _filmsRepositoryMock = new Mock<IFilmsRepository>();
        _mapperMock = new Mock<IMapper>();
        _notifierContext = new NotificationContext();
        _filmsService = new FilmsService(_filmsRepositoryMock.Object, _mapperMock.Object, _notifierContext);
    }

    [Fact]
    public void GetById_ShouldReturnNull_WhenSpecificationNotSatisfied()
    {
        // Arrange
        var filter = new FilterFilmsById { Id = Guid.NewGuid() };
        _filmsRepositoryMock.Setup(repo => repo.GetByElement(It.IsAny<FilterByItem>())).Returns((Films)null);

        // Act
        var result = _filmsService.GetById(filter);

        // Assert
        Assert.Null(result);
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
            TotalPages = 1,
            ItensList = new List<Films> { new Films { Id = Guid.NewGuid(), Name = "Test Film" } }
        };
        _filmsRepositoryMock.Setup(repo => repo.GetFilter(filter)).Returns(filterResult);
        _mapperMock.Setup(m => m.Map<IEnumerable<FilmsReadDto>>(It.IsAny<IEnumerable<Films>>()))
            .Returns(new List<FilmsReadDto> { new FilmsReadDto { Id = Guid.NewGuid(), Name = "Test Film" } });

        // Act
        var result = _filmsService.GetFilter(filter);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(filterResult.TotalRegister, result.TotalRegister);
        Assert.Equal(filterResult.TotalRegisterFilter, result.TotalRegisterFilter);
        Assert.Equal(filterResult.TotalPages, result.TotalPages);
        Assert.Equal(1, result.ItensList.Count());
    }

    [Fact]
    public void Add_ShouldReturnFilmsUpdateDto_WhenInputIsValid()
    {
        // Arrange
        var filmsCreateDto = new FilmsCreateDto { Name = "New Film", Duration = 120, AgeRange = 12, FilmGenres = FilmGenres.Action };
        var films = new Films { Id = Guid.NewGuid(), Name = "New Film" };
        var filmsUpdateDto = new FilmsUpdateDto { Id = films.Id, Name = "New Film", Duration = 120, AgeRange = 12, FilmGenres = FilmGenres.Action };

        _filmsRepositoryMock.Setup(repo => repo.ValidateInput(filmsCreateDto, false, null)).Returns(true);
        _mapperMock.Setup(m => m.Map<Films>(filmsCreateDto)).Returns(films);
        _filmsRepositoryMock.Setup(repo => repo.Add(films)).Returns(films);
        _mapperMock.Setup(m => m.Map<FilmsUpdateDto>(films)).Returns(filmsUpdateDto);

        // Act
        var result = _mapperMock.Object.Map<FilmsUpdateDto>(_filmsRepositoryMock.Object.Add(films));
    
        // Assert
        Assert.NotNull(result);
        Assert.IsType<FilmsUpdateDto>(result);
        Assert.Equal(films.Id, result.Id);
        Assert.Equal(filmsCreateDto.Name, result.Name);
    }

    [Fact]
    public void Update_ShouldCallRepositoryUpdate_WhenInputIsValid()
    {
        // Arrange
        Guid id = Guid.NewGuid();
        var filmsUpdateDto = new FilmsUpdateDto { Id = id, Name = "Updated Film", Duration = 130, AgeRange = 14, FilmGenres = FilmGenres.Comedy };
        var existingFilms = new Films { Id = id, Name = "Updated Film", Duration = 130, AgeRange = 14, FilmGenres = FilmGenres.Comedy };

        _filmsRepositoryMock.Setup(repo => repo.GetByElement(It.IsAny<FilterByItem>())).Returns(existingFilms);
        _filmsRepositoryMock.Setup(repo => repo.ValidateInput(filmsUpdateDto, true, existingFilms)).Returns(true);
        _mapperMock.Setup(m => m.Map<Films>(filmsUpdateDto)).Returns(existingFilms);

        // Act
        _filmsService.Update(filmsUpdateDto);

        // Assert
        _filmsRepositoryMock.Verify(repo => repo.Update(existingFilms), Times.Once);
    }

    [Fact]
    public void Delete_ShouldCallRepositoryDelete_WhenFilmExists()
    {
        // Arrange
        var filmId = Guid.NewGuid();
        var existingFilm = new Films { Id = filmId, Name = "Updated Film", Duration = 130, AgeRange = 14, FilmGenres = FilmGenres.Comedy };

        _filmsRepositoryMock.Setup(repo => repo.GetByElement(It.IsAny<FilterByItem>())).Returns(existingFilm);

        // Act
        _filmsService.Delete(filmId);

        // Assert
        _filmsRepositoryMock.Verify(repo => repo.Delete(existingFilm), Times.Once);
    }
}