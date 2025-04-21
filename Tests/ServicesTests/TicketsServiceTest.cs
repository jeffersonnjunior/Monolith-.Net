using Application.Dtos;
using Application.Interfaces.IFactory;
using Application.Services;
using Domain.Entities;
using Infrastructure.Interfaces.IRepositories;
using Infrastructure.Notifications;
using Infrastructure.FiltersModel;
using Moq;

public class TicketsServiceTests
{
    private readonly Mock<ITicketsRepository> _ticketsRepositoryMock;
    private readonly Mock<ITicketsFactory> _ticketsFactoryMock;
    private readonly NotificationContext _notificationContext;
    private readonly TicketsService _ticketsService;

    public TicketsServiceTests()
    {
        _ticketsRepositoryMock = new Mock<ITicketsRepository>();
        _ticketsFactoryMock = new Mock<ITicketsFactory>();
        _notificationContext = new NotificationContext();
        _ticketsService = new TicketsService(
            _ticketsRepositoryMock.Object,
            _notificationContext,
            _ticketsFactoryMock.Object
        );
    }

    [Fact]
    public void GetById_ShouldReturnTicketReadDto_WhenTicketExists()
    {
        // Arrange
        var ticketId = Guid.NewGuid();
        var ticket = new Tickets
        {
            Id = ticketId,
            SessionId = Guid.NewGuid(),
            SeatId = Guid.NewGuid(),
            CustomerDetailsId = Guid.NewGuid()
        };
        var ticketReadDto = new TicketsReadDto
        {
            Id = ticketId,
            SessionId = ticket.SessionId,
            SeatId = ticket.SeatId,
            CustomerDetailsId = ticket.CustomerDetailsId
        };

        _ticketsRepositoryMock
            .Setup(repo => repo.GetByElement(It.IsAny<FilterByItem>()))
            .Returns(ticket);

        _ticketsFactoryMock
            .Setup(factory => factory.MapToTicketReadDto(ticket))
            .Returns(ticketReadDto);

        var filter = new FilterTicketsById { Id = ticketId };

        // Act
        var result = _ticketsService.GetById(filter);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(ticketId, result.Id);
        Assert.Equal(ticket.SessionId, result.SessionId);
    }

    [Fact]
    public void Add_ShouldReturnTicketUpdateDto_WhenInputIsValid()
    {
        // Arrange
        var createDto = new TicketsCreateDto
        {
            SessionId = Guid.NewGuid(),
            SeatId = Guid.NewGuid(),
            CustomerDetailsId = Guid.NewGuid()
        };
        var ticket = new Tickets
        {
            Id = Guid.NewGuid(),
            SessionId = createDto.SessionId,
            SeatId = createDto.SeatId,
            CustomerDetailsId = createDto.CustomerDetailsId
        };
        var updateDto = new TicketsUpdateDto
        {
            Id = ticket.Id,
            SessionId = ticket.SessionId,
            SeatId = ticket.SeatId,
            CustomerDetailsId = ticket.CustomerDetailsId
        };

        _ticketsRepositoryMock
            .Setup(repo => repo.ValidateInput(createDto, false, null))
            .Returns(true);

        _ticketsFactoryMock
            .Setup(factory => factory.MapToTicket(createDto))
            .Returns(ticket);

        _ticketsRepositoryMock
            .Setup(repo => repo.Add(ticket))
            .Returns(ticket);

        _ticketsFactoryMock
            .Setup(factory => factory.MapToTicketUpdateDto(ticket))
            .Returns(updateDto);

        // Act
        var result = _ticketsService.Add(createDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(ticket.Id, result.Id);
        Assert.Equal(ticket.SessionId, result.SessionId);
    }

    [Fact]
    public void Delete_ShouldCallRepositoryDelete_WhenTicketExists()
    {
        // Arrange
        var ticketId = Guid.NewGuid();
        var ticket = new Tickets
        {
            Id = ticketId,
            SessionId = Guid.NewGuid(),
            SeatId = Guid.NewGuid(),
            CustomerDetailsId = Guid.NewGuid()
        };

        _ticketsRepositoryMock
            .Setup(repo => repo.GetByElement(It.IsAny<FilterByItem>()))
            .Returns(ticket);

        // Act
        _ticketsService.Delete(ticketId);

        // Assert
        _ticketsRepositoryMock.Verify(repo => repo.Delete(ticket), Times.Once);
    }
}