using System;
using Application.Dtos;
using Application.Specification;
using Infrastructure.Notifications;
using Xunit;

namespace SpecificationTest;

public class TheaterLocationSpecificationTest
{
    private readonly TheaterLocationSpecification _specification;

    public TheaterLocationSpecificationTest()
    {
        _specification = new TheaterLocationSpecification(new NotificationContext());
    }

    [Fact]
    public void IsSatisfiedBy_ShouldReturnTrue_ForValidCreateDto()
    {
        // Arrange
        var createDto = new TheaterLocationCreateDto
        {
            Street = "Valid Street",
            UnitNumber = "123",
            PostalCode = "12345"
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
        var createDto = new TheaterLocationCreateDto
        {
            Street = "",
            UnitNumber = "",
            PostalCode = ""
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
        var updateDto = new TheaterLocationUpdateDto
        {
            Id = Guid.NewGuid(),
            Street = "Valid Street",
            UnitNumber = "123",
            PostalCode = "12345"
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
        var updateDto = new TheaterLocationUpdateDto
        {
            Id = Guid.NewGuid(),
            Street = "",
            UnitNumber = "",
            PostalCode = ""
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