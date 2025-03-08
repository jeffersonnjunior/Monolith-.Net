using Application.Dtos;
using Infrastructure.Notifications;

namespace Application.Specification;

public class TicketsSpecification
{
    private readonly NotificationContext _notificationContext;

    public TicketsSpecification(NotificationContext notificationContext)
    {
        _notificationContext = notificationContext;
    }

    public bool IsSatisfiedBy(object dto)
    {
        return dto switch
        {
            TicketsCreateDto createDto => ValidateCreateDto(createDto),
            TicketsUpdateDto updateDto => ValidateUpdateDto(updateDto),
            _ => false
        };
    }

    public bool ValidateCreateDto(TicketsCreateDto createDto)
    {
        return ValidateSessionId(createDto.SessionId)
            && ValidateSeatId(createDto.SeatId)
            && ValidateClientId(createDto.ClientId);
    }

    public bool ValidateUpdateDto(TicketsUpdateDto updateDto)
    {
        return ValidateId(updateDto.Id)
            && ValidateSessionId(updateDto.SessionId)
            && ValidateSeatId(updateDto.SeatId)
            && ValidateClientId(updateDto.ClientId);
    }

    private bool ValidateId(Guid id)
    {
        if (id == Guid.Empty)
        {
            _notificationContext.AddNotification("Id é obrigatório.");
            return false;
        }
        return true;
    }

    private bool ValidateSessionId(Guid sessionId)
    {
        if (sessionId == Guid.Empty)
        {
            _notificationContext.AddNotification("SessionId é obrigatório.");
            return false;
        }
        return true;
    }

    private bool ValidateSeatId(Guid seatId)
    {
        if (seatId == Guid.Empty)
        {
            _notificationContext.AddNotification("SeatId é obrigatório.");
            return false;
        }
        return true;
    }

    private bool ValidateClientId(Guid clientId)
    {
        if (clientId == Guid.Empty)
        {
            _notificationContext.AddNotification("ClientId é obrigatório.");
            return false;
        }
        return true;
    }
}