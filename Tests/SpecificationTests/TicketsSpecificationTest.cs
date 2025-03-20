using System;
using Application.Dtos;
using Application.Specification;
using Infrastructure.Notifications;
using Xunit;

namespace SpecificationTest;

public class TicketsSpecificationTest
{
    private readonly TicketsSpecification _specification;

    public TicketsSpecificationTest()
    {
        _specification = new TicketsSpecification(new NotificationContext());
    }

    [Fact]
    public void IsSatisfiedBy_ShouldReturnTrue_ForValidCreateDto()
    {
        // Arrange
        var createDto = new TicketsCreateDto
        {
            SessionId = Guid.NewGuid(),
            SeatId = Guid.NewGuid(),
            ClientId = Guid.NewGuid()
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
        var createDto = new TicketsCreateDto
        {
            SessionId = Guid.Empty,
            SeatId = Guid.Empty,
            ClientId = Guid.Empty
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
        var updateDto = new TicketsUpdateDto
        {
            Id = Guid.NewGuid(),
            SessionId = Guid.NewGuid(),
            SeatId = Guid.NewGuid(),
            ClientId = Guid.NewGuid()
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
        var updateDto = new TicketsUpdateDto
        {
            Id = Guid.Empty,
            SessionId = Guid.Empty,
            SeatId = Guid.Empty,
            ClientId = Guid.Empty
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