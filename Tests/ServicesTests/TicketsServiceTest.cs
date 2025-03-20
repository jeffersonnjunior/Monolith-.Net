using Application.Dtos;
using Application.Interfaces.IServices;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces.IRepositories;
using Infrastructure.Notifications;
using Moq;
using System;
using System.Collections.Generic;
using Infrastructure.Utilities.FiltersModel;
using Xunit;

namespace ServicesTest;

public class TicketsServiceTests
{
    private readonly Mock<ITicketsRepository> _ticketsRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<NotificationContext> _notifierContextMock;
    private readonly TicketsService _ticketsService;

    public TicketsServiceTests()
    {
        _ticketsRepositoryMock = new Mock<ITicketsRepository>();
        _mapperMock = new Mock<IMapper>();
        _notifierContextMock = new Mock<NotificationContext>();
        _ticketsService = new TicketsService(_ticketsRepositoryMock.Object, _mapperMock.Object, _notifierContextMock.Object);
    }

    [Fact]
    public void GetById_ShouldReturnMappedDto_WhenTicketExists()
    {
        // Arrange
        var filter = new FilterTicketsById { Id = Guid.NewGuid() };
        var tickets = new Tickets { Id = filter.Id };
        var ticketsReadDto = new TicketsReadDto();
        
        _ticketsRepositoryMock.Setup(repo => repo.GetByElement(It.IsAny<FilterByItem>())).Returns(tickets);
        _mapperMock.Setup(m => m.Map<TicketsReadDto>(It.IsAny<Tickets>())).Returns(ticketsReadDto);

        // Act
        var result = _ticketsService.GetById(filter);

        // Assert
        Assert.Equal(ticketsReadDto, result);
        _ticketsRepositoryMock.Verify(repo => repo.GetByElement(It.IsAny<FilterByItem>()), Times.Once);
    }

    [Fact]
    public void GetFilter_ShouldReturnFilteredTickets_WhenFilterIsApplied()
    {
        // Arrange
        var filter = new FilterTicketsTable();
        var filterResult = new FilterReturn<Tickets>
        {
            TotalRegister = 10,
            TotalRegisterFilter = 5,
            TotalPages = 1,
            ItensList = new List<Tickets> { new Tickets() }
        };
        var ticketsReadDto = new TicketsReadDto();
        
        _ticketsRepositoryMock.Setup(repo => repo.GetFilter(It.IsAny<FilterTicketsTable>())).Returns(filterResult);
        _mapperMock.Setup(m => m.Map<IEnumerable<TicketsReadDto>>(It.IsAny<IEnumerable<Tickets>>())).Returns(new List<TicketsReadDto> { ticketsReadDto });

        // Act
        var result = _ticketsService.GetFilter(filter);

        // Assert
        Assert.Equal(10, result.TotalRegister);
        Assert.Equal(5, result.TotalRegisterFilter);
        Assert.Equal(1, result.TotalPages);
        Assert.Single(result.ItensList);
        _ticketsRepositoryMock.Verify(repo => repo.GetFilter(It.IsAny<FilterTicketsTable>()), Times.Once);
    }
    
    [Fact]
    public void Add_ShouldReturnNull_WhenSpecificationFails()
    {
        // Arrange
        var ticketsCreateDto = new TicketsCreateDto
        {
            ClientId = Guid.NewGuid(),
            SeatId = Guid.NewGuid(),
            SessionId = Guid.NewGuid()
        };

        // Act
        var result = _ticketsService.Add(ticketsCreateDto);

        // Assert
        Assert.Null(result); // Verifica se o retorno é null
        _ticketsRepositoryMock.Verify(repo => repo.Add(It.IsAny<Tickets>()), Times.Never); // Verifica se o método Add não foi chamado
    }
    
    [Fact]
    public void Update_ShouldNotCallRepository_WhenSpecificationFails()
    {
        // Arrange
        var ticketsUpdateDto = new TicketsUpdateDto
        {
            Id = Guid.NewGuid(),
            ClientId = Guid.NewGuid(),
            SeatId = Guid.NewGuid(),
            SessionId = Guid.NewGuid()
        };
        
        _ticketsRepositoryMock.Setup(repo => repo.Update(It.IsAny<Tickets>())).Verifiable();

        // Act
        _ticketsService.Update(ticketsUpdateDto);

        // Assert
        _ticketsRepositoryMock.Verify(repo => repo.Update(It.IsAny<Tickets>()), Times.Never);
    }


    [Fact]
    public void Delete_ShouldDeleteTicket_WhenTicketExists()
    {
        // Arrange
        var ticketId = Guid.NewGuid();
        var tickets = new Tickets { Id = ticketId };

        _ticketsRepositoryMock.Setup(repo => repo.GetByElement(It.IsAny<FilterByItem>())).Returns(tickets);
        _ticketsRepositoryMock.Setup(repo => repo.Delete(It.IsAny<Tickets>())).Verifiable();

        // Act
        _ticketsService.Delete(ticketId);

        // Assert
        _ticketsRepositoryMock.Verify(repo => repo.Delete(It.IsAny<Tickets>()), Times.Once);
    }

    [Fact]
    public void Delete_ShouldNotDeleteTicket_WhenTicketDoesNotExist()
    {
        // Arrange
        var ticketId = Guid.NewGuid();
        
        _ticketsRepositoryMock.Setup(repo => repo.GetByElement(It.IsAny<FilterByItem>())).Returns((Tickets)null);

        // Act
        _ticketsService.Delete(ticketId);

        // Assert
        _ticketsRepositoryMock.Verify(repo => repo.Delete(It.IsAny<Tickets>()), Times.Never);
    }
}
