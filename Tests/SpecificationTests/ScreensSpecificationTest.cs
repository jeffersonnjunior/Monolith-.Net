using Application.Dtos;
using Application.Specification;
using Infrastructure.FiltersModel;
using Infrastructure.Notifications;

namespace SpecificationTest;

public class ScreensSpecificationTest
{
    private readonly ScreensSpecification _specification;

    public ScreensSpecificationTest()
    {
        _specification = new ScreensSpecification(new NotificationContext());
    }

    [Fact]
    public void IsSatisfiedBy_ShouldReturnTrue_ForValidCreateDto()
    {
        // Arrange
        var createDto = new ScreensCreateDto
        {
            MovieTheaterId = Guid.NewGuid(),
            ScreenNumber = "A1",
            SeatingCapacity = 100
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
        var createDto = new ScreensCreateDto
        {
            MovieTheaterId = Guid.NewGuid(),
            ScreenNumber = "",
            SeatingCapacity = 0
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
        var updateDto = new ScreensUpdateDto
        {
            Id = Guid.NewGuid(),
            ScreenNumber = "B2",
            SeatingCapacity = 200,
            MovieTheaterId = Guid.NewGuid(),
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
        var updateDto = new ScreensUpdateDto
        {
            Id = Guid.Empty,
            ScreenNumber = "",
            SeatingCapacity = -50,
            MovieTheaterId = Guid.NewGuid(),
        };

        // Act
        var result = _specification.IsSatisfiedBy(updateDto);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsSatisfiedBy_ShouldReturnTrue_ForValidFilterScreensById()
    {
        // Arrange
        var filterDto = new FilterScreensById
        {
            Id = Guid.NewGuid(),
            Includes = new [] { "Seats" }
        };

        // Act
        var result = _specification.IsSatisfiedBy(filterDto);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsSatisfiedBy_ShouldReturnFalse_ForInvalidFilterScreensById()
    {
        // Arrange
        var filterDto = new FilterScreensById
        {
            Id = Guid.Empty,
            Includes = new [] { "InvalidProperty" }
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
