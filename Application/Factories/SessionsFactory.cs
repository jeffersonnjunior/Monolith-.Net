using Application.Dtos;
using Application.Interfaces.IFactories;
using Domain.Entities;

namespace Application.Factories;

public class SessionsFactory : ISessionsFactory
{
    public Sessions CreateSession()
    {
        return new Sessions
        {
            Id = Guid.NewGuid(),
            SessionTime = DateTime.MinValue,
            FilmId = Guid.Empty,
            FilmAudioOption = default,
            FilmFormat = default,
            Film = null,
            Tickets = new List<Tickets>()
        };
    }

    public SessionsCreateDto CreateSessionCreateDto()
    {
        return new SessionsCreateDto
        {
            SessionTime = DateTime.MinValue,
            FilmId = Guid.Empty,
            FilmAudioOption = default,
            FilmFormat = default
        };
    }

    public SessionsReadDto CreateSessionReadDto()
    {
        return new SessionsReadDto
        {
            Id = Guid.NewGuid(),
            SessionTime = DateTime.MinValue,
            FilmId = Guid.Empty,
            FilmAudioOption = default,
            FilmFormat = default
        };
    }

    public SessionsUpdateDto CreateSessionUpdateDto()
    {
        return new SessionsUpdateDto
        {
            Id = Guid.NewGuid(),
            SessionTime = DateTime.MinValue,
            FilmId = Guid.Empty,
            FilmAudioOption = default,
            FilmFormat = default
        };
    }

    public Sessions MapToSession(SessionsCreateDto dto)
    {
        return new Sessions
        {
            Id = Guid.NewGuid(),
            SessionTime = dto.SessionTime,
            FilmId = dto.FilmId,
            FilmAudioOption = dto.FilmAudioOption,
            FilmFormat = dto.FilmFormat,
            Tickets = new List<Tickets>()
        };
    }

    public SessionsCreateDto MapToSessionCreateDto(Sessions entity)
    {
        return new SessionsCreateDto
        {
            SessionTime = entity.SessionTime,
            FilmId = entity.FilmId,
            FilmAudioOption = entity.FilmAudioOption,
            FilmFormat = entity.FilmFormat
        };
    }

    public SessionsReadDto MapToSessionReadDto(Sessions entity)
    {
        return new SessionsReadDto
        {
            Id = entity.Id,
            SessionTime = entity.SessionTime,
            FilmId = entity.FilmId,
            FilmAudioOption = entity.FilmAudioOption,
            FilmFormat = entity.FilmFormat
        };
    }

    public SessionsUpdateDto MapToSessionUpdateDto(Sessions entity)
    {
        return new SessionsUpdateDto
        {
            Id = entity.Id,
            SessionTime = entity.SessionTime,
            FilmId = entity.FilmId,
            FilmAudioOption = entity.FilmAudioOption,
            FilmFormat = entity.FilmFormat
        };
    }

    public Sessions MapToSessionFromUpdateDto(SessionsUpdateDto dto)
    {
        return new Sessions
        {
            Id = dto.Id,
            SessionTime = dto.SessionTime,
            FilmId = dto.FilmId,
            FilmAudioOption = dto.FilmAudioOption,
            FilmFormat = dto.FilmFormat,
            Tickets = new List<Tickets>()
        };
    }
}