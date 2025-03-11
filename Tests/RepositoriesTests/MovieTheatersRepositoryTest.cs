using Application.Dtos;
using Domain.Entities;
using Infrastructure.Interfaces.IRepositories;
using Infrastructure.Notifications;
using Infrastructure.Utilities.FiltersModel;
using Moq;
using Xunit;

namespace RepositoryTest;

public class MovieTheatersRepositoryTest
{
    private readonly Mock<IMovieTheatersRepository> _movieTheatersRepositoryMock;
    private readonly Mock<NotificationContext> _notificationContextMock;

    public MovieTheatersRepositoryTest()
    {
        _movieTheatersRepositoryMock = new Mock<IMovieTheatersRepository>();
        _notificationContextMock = new Mock<NotificationContext>();
    }

    [Fact]
    public void GetByElement_ShouldReturnMovieTheater()
    {
        // Arrange
        var filterByItem = new FilterByItem { Field = "Id", Value = Guid.NewGuid(), Key = "Equal" };
        var movieTheater = new MovieTheaters { Name = "Test Theater" };

        _movieTheatersRepositoryMock.Setup(repo => repo.GetByElement(It.IsAny<FilterByItem>())).Returns(movieTheater);

        // Act
        var result = _movieTheatersRepositoryMock.Object.GetByElement(filterByItem);

        // Assert
        Assert.Equal(movieTheater, result);
    }

    [Fact]
    public void GetFilter_ShouldReturnFilterReturnOfMovieTheaters()
    {
        // Arrange
        var filter = new FilterMovieTheatersTable();
        var filterResult = new FilterReturn<MovieTheaters> { ItensList = new List<MovieTheaters>() };

        _movieTheatersRepositoryMock.Setup(repo => repo.GetFilter(filter)).Returns(filterResult);

        // Act
        var result = _movieTheatersRepositoryMock.Object.GetFilter(filter);

        // Assert
        Assert.Equal(filterResult.ItensList, result.ItensList);
    }

    [Fact]
    public void ValidateInput_ShouldReturnTrue_WhenNameIsNotInUse()
    {
        // Arrange
        var movieTheatersCreateDto = new { Name = "Test Theater" };
        var existingMovieTheater = new MovieTheaters { Name = "Another Theater" };

        _movieTheatersRepositoryMock.Setup(repo => repo.ValidateInput(movieTheatersCreateDto, false, existingMovieTheater)).Returns(true);

        // Act
        var result = _movieTheatersRepositoryMock.Object.ValidateInput(movieTheatersCreateDto, false, existingMovieTheater);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void ValidateInput_ShouldReturnFalse_WhenNameIsInUse()
    {
        // Arrange
        var movieTheatersCreateDto = new { Name = "Test Theater" };
        var existingMovieTheater = new MovieTheaters { Name = "Test Theater" };

        _movieTheatersRepositoryMock.Setup(repo => repo.ValidateInput(movieTheatersCreateDto, false, existingMovieTheater)).Returns(false);

        // Act
        var result = _movieTheatersRepositoryMock.Object.ValidateInput(movieTheatersCreateDto, false, existingMovieTheater);

        // Assert
        Assert.False(result);
    }
}