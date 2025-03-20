using System;
using Application.Dtos;
using Application.Specification;
using Infrastructure.Notifications;
using Xunit;

namespace SpecificationTest;

public class CustomerDetailsSpecificationTest
{
    private readonly CustomerDetailsSpecification _specification;

    public CustomerDetailsSpecificationTest()
    {
        _specification = new CustomerDetailsSpecification(new NotificationContext());
    }

    [Fact]
    public void IsSatisfiedBy_ShouldReturnTrue_ForValidCreateDto()
    {
        // Arrange
        var createDto = new CustomerDetailsCreateDto
        {
            Name = "John Doe",
            Email = "johndoe@example.com",
            Age = 30
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
        var createDto = new CustomerDetailsCreateDto
        {
            Name = "",
            Email = "invalid-email",
            Age = -1
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
        var updateDto = new CustomerDetailsUpdateDto
        {
            Id = Guid.NewGuid(),
            Name = "Jane Doe",
            Email = "janedoe@example.com",
            Age = 25
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
        var updateDto = new CustomerDetailsUpdateDto
        {
            Id = Guid.Empty,
            Name = "",
            Email = "invalid-email",
            Age = -5
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
