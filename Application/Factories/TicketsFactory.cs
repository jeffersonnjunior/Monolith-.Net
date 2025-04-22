using Application.Dtos;
using Application.Interfaces.IFactories;
using Domain.Entities;

namespace Application.Factories;

public class TicketsFactory : ITicketsFactory
{
    public Tickets CreateTicket()
    {
        return new Tickets
        {
            Id = Guid.NewGuid(),
            SessionId = Guid.Empty,
            SeatId = Guid.Empty,
            CustomerDetailsId = Guid.Empty,
            Session = null,
            Seat = null,
            CustomerDetails = null
        };
    }

    public TicketsCreateDto CreateTicketCreateDto()
    {
        return new TicketsCreateDto
        {
            SessionId = Guid.Empty,
            SeatId = Guid.Empty,
            CustomerDetailsId = Guid.Empty
        };
    }

    public TicketsReadDto CreateTicketReadDto()
    {
        return new TicketsReadDto
        {
            Id = Guid.NewGuid(),
            SessionId = Guid.Empty,
            SeatId = Guid.Empty,
            CustomerDetailsId = Guid.Empty
        };
    }

    public TicketsUpdateDto CreateTicketUpdateDto()
    {
        return new TicketsUpdateDto
        {
            Id = Guid.NewGuid(),
            SessionId = Guid.Empty,
            SeatId = Guid.Empty,
            CustomerDetailsId = Guid.Empty
        };
    }

    public Tickets MapToTicket(TicketsCreateDto dto)
    {
        return new Tickets
        {
            Id = Guid.NewGuid(),
            SessionId = dto.SessionId,
            SeatId = dto.SeatId,
            CustomerDetailsId = dto.CustomerDetailsId
        };
    }

    public TicketsCreateDto MapToTicketCreateDto(Tickets entity)
    {
        return new TicketsCreateDto
        {
            SessionId = entity.SessionId,
            SeatId = entity.SeatId,
            CustomerDetailsId = entity.CustomerDetailsId
        };
    }

    public TicketsReadDto MapToTicketReadDto(Tickets entity)
    {
        return new TicketsReadDto
        {
            Id = entity.Id,
            SessionId = entity.SessionId,
            SeatId = entity.SeatId,
            CustomerDetailsId = entity.CustomerDetailsId
        };
    }

    public TicketsUpdateDto MapToTicketUpdateDto(Tickets entity)
    {
        return new TicketsUpdateDto
        {
            Id = entity.Id,
            SessionId = entity.SessionId,
            SeatId = entity.SeatId,
            CustomerDetailsId = entity.CustomerDetailsId
        };
    }

    public Tickets MapToTicketFromUpdateDto(TicketsUpdateDto dto)
    {
        return new Tickets
        {
            Id = dto.Id,
            SessionId = dto.SessionId,
            SeatId = dto.SeatId,
            CustomerDetailsId = dto.CustomerDetailsId
        };
    }
}