using Application.Dtos;
using Infrastructure.FiltersModel;

namespace Application.Interfaces.IServices;

public interface ITicketsService
{
    TicketsReadDto GetById(FilterTicketsById filterTicketsById);
    FilterReturn<TicketsReadDto> GetFilter(FilterTicketsTable filter);
    TicketsUpdateDto Add(TicketsCreateDto ticketsCreateDto);
    void Update(TicketsUpdateDto ticketsUpdateDto);
    void Delete(Guid id);
}
