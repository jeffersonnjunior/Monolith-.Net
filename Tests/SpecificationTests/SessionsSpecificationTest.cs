using System;
using System.Collections.Generic;
using Application.Dtos;
using Application.Specification;
using Domain.Enums;
using Infrastructure.Notifications;
using Infrastructure.Utilities.FiltersModel;
using Xunit;

namespace SpecificationTest;

public class SessionsSpecificationTest
{
    private readonly SessionsSpecification _specification;

    public SessionsSpecificationTest()
    {
        _specification = new SessionsSpecification(new NotificationContext());
    }

    [Fact]
    public void IsSatisfiedBy_ShouldReturnTrue_ForValidCreateDto()
    {
        // Arrange
        var createDto = new SessionsCreateDto
        {
            SessionTime = DateTime.Now,
            FilmId = Guid.NewGuid(),
            FilmAudioOption = FilmAudioOption.Dubbed,
            FilmFormat = FilmFormat.D3
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
        var createDto = new SessionsCreateDto
        {
            SessionTime = DateTime.MinValue,
            FilmId = Guid.Empty,
            FilmAudioOption = (FilmAudioOption)999,
            FilmFormat = (FilmFormat)999
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
        var updateDto = new SessionsUpdateDto
        {
            Id = Guid.NewGuid(),
            SessionTime = DateTime.Now,
            FilmId = Guid.NewGuid(),
            FilmAudioOption = FilmAudioOption.Dubbed,
            FilmFormat = FilmFormat.D3
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
        var updateDto = new SessionsUpdateDto
        {
            Id = Guid.Empty,
            SessionTime = DateTime.MinValue,
            FilmId = Guid.Empty,
            FilmAudioOption = (FilmAudioOption)999,
            FilmFormat = (FilmFormat)999
        };

        // Act
        var result = _specification.IsSatisfiedBy(updateDto);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsSatisfiedBy_ShouldReturnTrue_ForValidFilterSessionsById()
    {
        // Arrange
        var filterDto = new FilterSessionsById
        {
            Id = Guid.NewGuid(),
            Includes = new[] { "Tickets" }
        };

        // Act
        var result = _specification.IsSatisfiedBy(filterDto);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsSatisfiedBy_ShouldReturnFalse_ForInvalidFilterSessionsById()
    {
        // Arrange
        var filterDto = new FilterSessionsById
        {
            Id = Guid.Empty,
            Includes = new[] { "InvalidProperty" }
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