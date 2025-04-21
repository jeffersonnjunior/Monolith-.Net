using Application.Dtos;
using Infrastructure.FiltersModel;

namespace Application.Interfaces.IServices;

public interface ISeatsService
{
    SeatsReadDto GetById(FilterSeatsById filterSeatsById);
    FilterReturn<SeatsReadDto> GetFilter(FilterSeatsTable filter);
    SeatsUpdateDto Add(SeatsCreateDto seatsCreateDto);
    void Update(SeatsUpdateDto seatsUpdateDto);
    void Delete(Guid id);
}
