using Application.Dtos;
using Domain.Entities;

namespace Application.Interfaces.IFactories;

public interface ISessionsFactory
{
    Sessions CreateSession();
    SessionsCreateDto CreateSessionCreateDto();
    SessionsReadDto CreateSessionReadDto();
    SessionsUpdateDto CreateSessionUpdateDto();
    Sessions MapToSession(SessionsCreateDto dto);
    SessionsCreateDto MapToSessionCreateDto(Sessions entity);
    SessionsReadDto MapToSessionReadDto(Sessions entity);
    SessionsUpdateDto MapToSessionUpdateDto(Sessions entity);
    Sessions MapToSessionFromUpdateDto(SessionsUpdateDto dto);
}