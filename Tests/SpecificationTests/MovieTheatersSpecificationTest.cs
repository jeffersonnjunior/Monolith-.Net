using System;
using Application.Dtos;
using Application.Specification;
using Infrastructure.Notifications;
using Xunit;

namespace SpecificationTest;

public class MovieTheatersSpecificationTest
{
    private readonly MovieTheatersSpecification _specification;

    public MovieTheatersSpecificationTest()
    {
        _specification = new MovieTheatersSpecification(new NotificationContext());
    }

    [Fact]
    public void IsSatisfiedBy_ShouldReturnTrue_ForValidCreateDto()
    {
        // Arrange
        var createDto = new MovieTheatersCreateDto
        {
            Name = "CineMax",
            TheaterLocationId = Guid.NewGuid()
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
        var createDto = new MovieTheatersCreateDto
        {
            Name = "",
            TheaterLocationId = Guid.Empty
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
        var updateDto = new MovieTheatersUpdateDto
        {
            Id = Guid.NewGuid(),
            Name = "CineMax Updated",
            TheaterLocationId = Guid.NewGuid()
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
        var updateDto = new MovieTheatersUpdateDto
        {
            Id = Guid.NewGuid(),
            Name = "",
            TheaterLocationId = Guid.Empty
        };

        // Act
        var result = _specification.IsSatisfiedBy(updateDto);

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
