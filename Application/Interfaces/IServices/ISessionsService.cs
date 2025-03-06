using Application.Dtos;
using Infrastructure.Utilities.FiltersModel;

namespace Application.Interfaces.IServices;

public interface ISessionsService
{
    SessionsReadDto GetById(FilterSessionsById filterSessionsById);
    FilterReturn<SessionsReadDto> GetFilter(FilterSessionsTable filter);
    SessionsUpdateDto Add(SessionsCreateDto sessionsCreateDto);
    void Update(SessionsUpdateDto sessionsUpdateDto);
    void Delete (Guid id);
}
