using Application.Dtos;
using Application.Interfaces.ISpecification;
using Infrastructure.Notifications;

namespace Application.Specification;

public class MovieTheatersSpecification<T> : ISpecificationBase<T>
    where T : class
{
    private readonly NotificationContext _notificationContext;
    
    public MovieTheatersSpecification(NotificationContext notificationContext)
    {
        _notificationContext = notificationContext;
    }

    public bool IsSatisfiedBy(T dto)
    {
        if (dto is MovieTheatersCreateDto createDto) return ValidateCreateDto(createDto);
        else if (dto is MovieTheatersUpdateDto updateDto) return ValidateUpdateDto(updateDto);
        else return false;
    }

    private bool ValidateCreateDto(MovieTheatersCreateDto dto)
    {
        bool isValid = true;
        isValid &= IsNameValid(dto.Name);
        isValid &= IsAddressIdValid(dto.TheaterLocationId);
        return isValid;
    }

    private bool ValidateUpdateDto(MovieTheatersUpdateDto dto)
    {
        bool isValid = true;
        isValid &= IsNameValid(dto.Name);
        isValid &= IsAddressIdValid(dto.TheaterLocationId);
        return isValid;
    }
    private bool IsNameValid(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            _notificationContext.AddNotification("O nome do cinema não pode estar vazio");
            return false;
        }
        return true;
    }

    private bool IsAddressIdValid(Guid addressId)
    {
        if (addressId == Guid.Empty)
        {
            _notificationContext.AddNotification("O endereço do cinema não pode estar vazio");
            return false;
        }
        return true;
    }
}