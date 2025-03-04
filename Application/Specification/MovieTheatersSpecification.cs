using Application.Dtos;
using Infrastructure.Notifications;

namespace Application.Specification;

public class MovieTheatersSpecification
{
    private readonly NotificationContext _notificationContext;
    
    public MovieTheatersSpecification(NotificationContext notificationContext)
    {
        _notificationContext = notificationContext;
    }

    public bool IsSatisfiedBy(object dto)
    {
        return dto switch
        {
            MovieTheatersCreateDto createDto => ValidateCreateDto(createDto),
            MovieTheatersUpdateDto updateDto => ValidateUpdateDto(updateDto),
            _ => false
        };
    }

    private bool ValidateCreateDto(MovieTheatersCreateDto dto)
    {
        bool isValid = true;
        isValid &= ValidateName(dto.Name);
        isValid &= ValidateAddressId(dto.TheaterLocationId);
        return isValid;
    }

    private bool ValidateUpdateDto(MovieTheatersUpdateDto dto)
    {
        bool isValid = true;
        isValid &= ValidateName(dto.Name);
        isValid &= ValidateAddressId(dto.TheaterLocationId);
        return isValid;
    }
    private bool ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            _notificationContext.AddNotification("O nome do cinema não pode estar vazio");
            return false;
        }
        return true;
    }

    private bool ValidateAddressId(Guid addressId)
    {
        if (addressId == Guid.Empty)
        {
            _notificationContext.AddNotification("O endereço do cinema não pode estar vazio");
            return false;
        }
        return true;
    }
}