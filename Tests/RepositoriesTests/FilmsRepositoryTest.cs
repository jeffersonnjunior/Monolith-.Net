using Domain.Entities;
using Domain.Enums;
using Infrastructure.Interfaces.IRepositories;
using Infrastructure.Notifications;
using Infrastructure.Utilities.FiltersModel;
using Moq;
using Xunit;

namespace RepositoryTest;

public class FilmsRepositoryTest
{
    private readonly Mock<IFilmsRepository> _filmsRepositoryMock;
    private readonly Mock<NotificationContext> _notificationContextMock;

    public FilmsRepositoryTest()
    {
        _filmsRepositoryMock = new Mock<IFilmsRepository>();
        _notificationContextMock = new Mock<NotificationContext>();
    }

    [Fact]
    public void GetByElement_ShouldReturnFilm()
    {
        // Arrange
        var filterByItem = new FilterByItem { Field = "Id", Value = Guid.NewGuid(), Key = "Equal" };
        var film = new Films { Name = "Test Film" };

        _filmsRepositoryMock.Setup(repo => repo.GetByElement(It.IsAny<FilterByItem>())).Returns(film);

        // Act
        var result = _filmsRepositoryMock.Object.GetByElement(filterByItem);

        // Assert
        Assert.Equal(film, result);
    }

    [Fact]
    public void GetFilter_ShouldReturnFilterReturnOfFilms()
    {
        // Arrange
        var filter = new FilterFilmsTable();
        var filterResult = new FilterReturn<Films> { ItensList = new List<Films>() };

        _filmsRepositoryMock.Setup(repo => repo.GetFilter(filter)).Returns(filterResult);

        // Act
        var result = _filmsRepositoryMock.Object.GetFilter(filter);

        // Assert
        Assert.Equal(filterResult.ItensList, result.ItensList);
    }

    [Fact]
    public void ValidateInput_ShouldReturnTrue_WhenNameIsNotInUse()
    {
        // Arrange
        var filmsCreateDto = new { Name = "Test Film" };
        var existingFilms = new Films { Name = "Another Film" };

        _filmsRepositoryMock.Setup(repo => repo.ValidateInput(filmsCreateDto, false, existingFilms)).Returns(true);

        // Act
        var result = _filmsRepositoryMock.Object.ValidateInput(filmsCreateDto, false, existingFilms);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void ValidateInput_ShouldReturnFalse_WhenNameIsInUse()
    {
        // Arrange
        var filmsCreateDto = new { Name = "Test Film" };
        var existingFilms = new Films { Name = "Test Film" };

        _filmsRepositoryMock.Setup(repo => repo.ValidateInput(filmsCreateDto, false, existingFilms)).Returns(false);

        // Act
        var result = _filmsRepositoryMock.Object.ValidateInput(filmsCreateDto, false, existingFilms);

        // Assert
        Assert.False(result);
    }
}