using Application.Dtos;
using Domain.Entities;

namespace Application.Interfaces.IFactory;

public interface ITicketsFactory
{
    Tickets CreateTicket();
    TicketsCreateDto CreateTicketCreateDto();
    TicketsReadDto CreateTicketReadDto();
    TicketsUpdateDto CreateTicketUpdateDto();
    Tickets MapToTicket(TicketsCreateDto dto);
    TicketsCreateDto MapToTicketCreateDto(Tickets entity);
    TicketsReadDto MapToTicketReadDto(Tickets entity);
    TicketsUpdateDto MapToTicketUpdateDto(Tickets entity);
    Tickets MapToTicketFromUpdateDto(TicketsUpdateDto dto);
}