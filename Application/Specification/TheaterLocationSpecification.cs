using Application.Dtos;
using Infrastructure.Notifications;

namespace Application.Specification;

public class TheaterLocationSpecification
{
    private readonly NotificationContext _notificationContext;
    public TheaterLocationSpecification(NotificationContext notificationContext)
    {
        _notificationContext = notificationContext;
    }

    public bool IsSatisfiedBy(object dto)
        => dto switch
        {
            TheaterLocationCreateDto createDto => ValidateCreateDto(createDto),
            TheaterLocationUpdateDto updateDto => ValidateUpdateDto(updateDto),
            _ => false
        };

    private bool ValidateCreateDto(TheaterLocationCreateDto dto)
    {
        bool isValid = true;
        isValid &= ValidateStreet(dto.Street);
        isValid &= ValidateUnitNumber(dto.UnitNumber);
        isValid &= ValidatePostalCode(dto.PostalCode);
        return isValid;
    }

    private bool ValidateUpdateDto(TheaterLocationUpdateDto dto)
    {
        bool isValid = true;
        isValid &= ValidateStreet(dto.Street);
        isValid &= ValidateUnitNumber(dto.UnitNumber);
        isValid &= ValidatePostalCode(dto.PostalCode);
        return isValid;
    }

    private bool ValidateStreet(string street)
    {
        if (string.IsNullOrWhiteSpace(street))
        {
            _notificationContext.AddNotification("A rua não pode estar vazia.");
            return false;
        }
        return true;
    }

    private bool ValidateUnitNumber(string unitNumber)
    {
        if (string.IsNullOrWhiteSpace(unitNumber))
        {
            _notificationContext.AddNotification("O número da unidade não pode estar vazio");
            return false;
        }
        return true;
    }

    private bool ValidatePostalCode(string postalCode)
    {
        if (string.IsNullOrWhiteSpace(postalCode))
        {
            _notificationContext.AddNotification("O código postal não pode estar vazio.");
            return false;
        }
        return true;
    }
}
