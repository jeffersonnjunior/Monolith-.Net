using Application.Dtos;
using Application.Specification;
using Infrastructure.Notifications;
using Infrastructure.FiltersModel;

namespace SpecificationTest;

public class SeatsSpecificationTest
{
    private readonly SeatsSpecification _specification;

    public SeatsSpecificationTest()
    {
        _specification = new SeatsSpecification(new NotificationContext());
    }

    [Fact]
    public void ValidateCreateDto_ValidData_ReturnsTrue()
    {
        // Arrange
        var dto = new SeatsCreateDto
        {
            SeatNumber = 1,
            RowLetter = "A",
            ScreenId = Guid.NewGuid()
        };

        // Act
        var result = _specification.IsSatisfiedBy(dto);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void ValidateCreateDto_InvalidSeatNumber_ReturnsFalse()
    {
        // Arrange
        var dto = new SeatsCreateDto
        {
            SeatNumber = 0, // Invalid
            RowLetter = "A",
            ScreenId = Guid.NewGuid()
        };

        // Act
        var result = _specification.IsSatisfiedBy(dto);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ValidateCreateDto_EmptyRowLetter_ReturnsFalse()
    {
        // Arrange
        var dto = new SeatsCreateDto
        {
            SeatNumber = 1,
            RowLetter = string.Empty, // Invalid
            ScreenId = Guid.NewGuid()
        };

        // Act
        var result = _specification.IsSatisfiedBy(dto);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ValidateCreateDto_EmptyScreenId_ReturnsFalse()
    {
        // Arrange
        var dto = new SeatsCreateDto
        {
            SeatNumber = 1,
            RowLetter = "A",
            ScreenId = Guid.Empty // Invalid
        };

        // Act
        var result = _specification.IsSatisfiedBy(dto);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ValidateUpdateDto_ValidData_ReturnsTrue()
    {
        // Arrange
        var dto = new SeatsUpdateDto
        {
            Id = Guid.NewGuid(),
            SeatNumber = 1,
            RowLetter = "A",
            ScreenId = Guid.NewGuid()
        };

        // Act
        var result = _specification.IsSatisfiedBy(dto);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void ValidateUpdateDto_EmptyId_ReturnsFalse()
    {
        // Arrange
        var dto = new SeatsUpdateDto
        {
            Id = Guid.Empty, 
            SeatNumber = 1,
            RowLetter = "A",
            ScreenId = Guid.NewGuid()
        };

        // Act
        var result = _specification.IsSatisfiedBy(dto);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void ValidateFilterSeatsById_ValidData_ReturnsTrue()
    {
        // Arrange
        var filter = new FilterSeatsById
        {
            Id = Guid.NewGuid(),
            Includes = new[] { "Tickets" }
        };

        // Act
        var result = _specification.IsSatisfiedBy(filter);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void ValidateFilterSeatsById_InvalidId_ReturnsFalse()
    {
        // Arrange
        var filter = new FilterSeatsById
        {
            Id = Guid.Empty,
            Includes = new[] { "Property1", "Property2" }
        };

        // Act
        var result = _specification.IsSatisfiedBy(filter);

        // Assert
        Assert.False(result);
    }
}
