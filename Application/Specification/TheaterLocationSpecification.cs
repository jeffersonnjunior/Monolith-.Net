using Application.Dtos;
using Application.Interfaces.ISpecification;
using Infrastructure.Notifications;

namespace Application.Specification;

public class TheaterLocationSpecification<T> : ISpecificationBase<T>
    where T : class
{
    private readonly NotificationContext _notificationContext;
    public TheaterLocationSpecification(NotificationContext notificationContext)
    {
        _notificationContext = notificationContext;
    }

    public bool IsSatisfiedBy(T dto)
    {
        if (dto is TheaterLocationCreateDto createDto) return ValidateCreateDto(createDto);
        
        else if (dto is TheaterLocationUpdateDto updateDto) return ValidateUpdateDto(updateDto);
        
        else return false;
        
    }

    private bool ValidateCreateDto(TheaterLocationCreateDto dto)
    {
        bool isValid = true;
        isValid &= IsStreetValid(dto.Street);
        isValid &= IsUnitNumberValid(dto.UnitNumber);
        isValid &= IsPostalCodeValid(dto.PostalCode);
        return isValid;
    }

    private bool ValidateUpdateDto(TheaterLocationUpdateDto dto)
    {
        bool isValid = true;
        isValid &= IsStreetValid(dto.Street);
        isValid &= IsUnitNumberValid(dto.UnitNumber);
        isValid &= IsPostalCodeValid(dto.PostalCode);
        return isValid;
    }

    private bool IsStreetValid(string street)
    {
        if (string.IsNullOrWhiteSpace(street))
        {
            _notificationContext.AddNotification("A rua não pode estar vazia.");
            return false;
        }
        return true;
    }

    private bool IsUnitNumberValid(string unitNumber)
    {
        if (string.IsNullOrWhiteSpace(unitNumber))
        {
            _notificationContext.AddNotification("O número da unidade não pode estar vazio");
            return false;
        }
        return true;
    }

    private bool IsPostalCodeValid(string postalCode)
    {
        if (string.IsNullOrWhiteSpace(postalCode))
        {
            _notificationContext.AddNotification("O código postal não pode estar vazio.");
            return false;
        }
        return true;
    }
}