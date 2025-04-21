using Application.Dtos;
using Application.Specification;
using Domain.Enums;
using Infrastructure.Notifications;
using Infrastructure.FiltersModel;

namespace SpecificationTest;

public class FilmsSpecificationTests
{
    private readonly FilmsSpecification _specification;

    public FilmsSpecificationTests()
    {
        _specification = new FilmsSpecification(new NotificationContext());
    }

    [Fact]
    public void IsSatisfiedBy_ShouldReturnTrue_ForValidCreateDto()
    {
        // Arrange
        var createDto = new FilmsCreateDto
        {
            Name = "Inception",
            Duration = 148,
            AgeRange = 13,
            FilmGenres = FilmGenres.ScienceFiction
        };

        // Act
        var result = _specification.IsSatisfiedBy(createDto);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsSatisfiedBy_ShouldReturnFalse_ForInvalidCreateDto()
    {
        // Arrange
        var createDto = new FilmsCreateDto
        {
            Name = "",
            Duration = -10,
            AgeRange = -1,
            FilmGenres = (FilmGenres)999
        };

        // Act
        var result = _specification.IsSatisfiedBy(createDto);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsSatisfiedBy_ShouldReturnTrue_ForValidUpdateDto()
    {
        // Arrange
        var updateDto = new FilmsUpdateDto
        {
            Id = Guid.NewGuid(),
            Name = "Interstellar",
            Duration = 169,
            AgeRange = 10,
            FilmGenres = FilmGenres.Drama
        };

        // Act
        var result = _specification.IsSatisfiedBy(updateDto);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsSatisfiedBy_ShouldReturnFalse_ForInvalidUpdateDto()
    {
        // Arrange
        var updateDto = new FilmsUpdateDto
        {
            Id = Guid.Empty,
            Name = "",
            Duration = 0,
            AgeRange = -5,
            FilmGenres = (FilmGenres)999
        };

        // Act
        var result = _specification.IsSatisfiedBy(updateDto);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsSatisfiedBy_ShouldReturnTrue_ForValidFilterFilmsById()
    {
        // Arrange
        var filterDto = new FilterFilmsById
        {
            Id = Guid.NewGuid(),
            Includes = new[] { "Name", "Duration" }
        };

        // Act
        var result = _specification.IsSatisfiedBy(filterDto);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsSatisfiedBy_ShouldReturnFalse_ForInvalidFilterFilmsById()
    {
        // Arrange
        var filterDto = new FilterFilmsById
        {
            Id = Guid.Empty,
            Includes = new[] { "InvalidProperty" } // Mudança para array de strings
        };

        // Act
        var result = _specification.IsSatisfiedBy(filterDto);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsSatisfiedBy_ShouldReturnFalse_ForUnsupportedDtoType()
    {
        // Arrange
        var unsupportedDto = new { Property = "Value" };

        // Act
        var result = _specification.IsSatisfiedBy(unsupportedDto);

        // Assert
        Assert.False(result);
    }
}
