using System.Reflection;
using Application.Dtos;
using Infrastructure.Notifications;
using Infrastructure.Utilities.FiltersModel;

namespace Application.Specification;

public class SeatsSpecification
{
    private readonly NotificationContext _notificationContext;

    public SeatsSpecification(NotificationContext notificationContext)
    {
        _notificationContext = notificationContext;
    }

    public bool IsSatisfiedBy(object dto)
    {
        return dto switch
        {
            SeatsCreateDto createDto => ValidateCreateDto(createDto),
            SeatsUpdateDto updateDto => ValidateUpdateDto(updateDto),
            FilterSeatsById filterSeatsById => ValidateFilterSeatsById(filterSeatsById),
            _ => false
        };
    }

    private bool ValidateCreateDto(SeatsCreateDto dto)
    {
        bool isValid = true;
        isValid &= ValidateSeatNumber(dto.SeatNumber);
        isValid &= ValidateRowLetter(dto.RowLetter);
        isValid &= ValidateScreenId(dto.ScreenId);
        return isValid;
    }

    private bool ValidateUpdateDto(SeatsUpdateDto dto)
    {
        bool isValid = true;
        isValid &= ValidateId(dto.Id);
        isValid &= ValidateSeatNumber(dto.SeatNumber);
        isValid &= ValidateRowLetter(dto.RowLetter);
        isValid &= ValidateScreenId(dto.ScreenId);
        return isValid;
    }

    private bool ValidateFilterSeatsById(FilterSeatsById filterSeatsById)
    {
        bool isValid = true;
        isValid &= ValidateId(filterSeatsById.Id);
        isValid &= ValidateIncludes(filterSeatsById.Includes);
        return isValid;
    }
    
    private bool ValidateId(Guid id)
    {
        if (id == Guid.Empty)
        {
            _notificationContext.AddNotification("O ID não pode estar vazio.");
            return false;
        }
        return true;
    }
    
    private bool ValidateIncludes(string[]? includes)
    {
        if (includes == null || !includes.Any())
        {
            return true;
        }

        var validProperties = typeof(SeatsReadDto).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(p => p.Name)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var invalidIncludes = includes.Where(include => !validProperties.Contains(include)).ToList();

        if (invalidIncludes.Any())
        {
            _notificationContext.AddNotification($"Esses Includes ({string.Join(", ", invalidIncludes)}) não são válidos.");
            return false;
        }

        return true;
    }
    
    private bool ValidateSeatNumber(int seatNumber)
    {
        if (seatNumber < 1)
        {
            _notificationContext.AddNotification("O número do assento deve ser maior que zero.");
            return false;
        }
        return true;
    }

    private bool ValidateRowLetter(string rowLetter)
    {
        if (string.IsNullOrWhiteSpace(rowLetter))
        {
            _notificationContext.AddNotification("A letra da fileira não pode estar vazia.");
            return false;
        }
        return true;
    }

    private bool ValidateScreenId(Guid screenId)
    {
        if (screenId == Guid.Empty)
        {
            _notificationContext.AddNotification("O ScreenId não pode estar vazio.");
            return false;
        }
        return true;
    }
}
