using Domain.Entities;
using Infrastructure.Interfaces.IRepositories;
using Infrastructure.Notifications;
using Infrastructure.Utilities.FiltersModel;
using Moq;
using Xunit;

namespace RepositoryTest;

public class TicketsRepositoryTest
{
    private readonly Mock<ITicketsRepository> _ticketsRepositoryMock;
    private readonly Mock<NotificationContext> _notificationContextMock;

    public TicketsRepositoryTest()
    {
        _ticketsRepositoryMock = new Mock<ITicketsRepository>();
        _notificationContextMock = new Mock<NotificationContext>();
    }

    [Fact]
    public void GetByElement_ShouldReturnTicket()
    {
        // Arrange
        var filterByItem = new FilterByItem { Field = "Id", Value = Guid.NewGuid(), Key = "Equal" };
        var ticket = new Tickets { Id = (Guid)filterByItem.Value, CustomerDetailsId = Guid.NewGuid(), SeatId = Guid.NewGuid(), SessionId = Guid.NewGuid() };

        _ticketsRepositoryMock.Setup(repo => repo.GetByElement(It.IsAny<FilterByItem>())).Returns(ticket);

        // Act
        var result = _ticketsRepositoryMock.Object.GetByElement(filterByItem);

        // Assert
        Assert.Equal(ticket, result);
    }

    [Fact]
    public void GetFilter_ShouldReturnFilterReturnOfTickets()
    {
        // Arrange
        var filter = new FilterTicketsTable();
        var filterResult = new FilterReturn<Tickets> { ItensList = new List<Tickets>() };

        _ticketsRepositoryMock.Setup(repo => repo.GetFilter(filter)).Returns(filterResult);

        // Act
        var result = _ticketsRepositoryMock.Object.GetFilter(filter);

        // Assert
        Assert.Equal(filterResult.ItensList, result.ItensList);
    }
}