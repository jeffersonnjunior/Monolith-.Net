using Application.Dtos;
using Application.Interfaces.ISpecification;
using Infrastructure.Notifications;

namespace Application.Specification
{
    public class TheaterLocationSpecification : ISpecificationBase<TheaterLocationDto>
    {
        private readonly NotificationContext _notificationContext;

        public TheaterLocationSpecification(NotificationContext notificationContext)
        {
            _notificationContext = notificationContext;
        }

        public bool IsSatisfiedBy(TheaterLocationDto theaterLocationDto)
        {
            bool isValid = true;

            isValid &= IsStreetValid(theaterLocationDto);
            isValid &= IsUnitNumberValid(theaterLocationDto);
            isValid &= IsPostalCodeValid(theaterLocationDto);

            return isValid; 
        }

        private bool IsStreetValid(TheaterLocationDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Street))
            {
                _notificationContext.AddNotification("A rua não pode estar vazia.");
                return false;
            }
            return true;
        }

        private bool IsUnitNumberValid(TheaterLocationDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.UnitNumber))
            {
                _notificationContext.AddNotification("O número da unidade não pode estar vazio.");
                return false;
            }
            return true;
        }

        private bool IsPostalCodeValid(TheaterLocationDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.PostalCode))
            {
                _notificationContext.AddNotification("O código postal não pode estar vazio.");
                return false;
            }
            return true;
        }
    }
}