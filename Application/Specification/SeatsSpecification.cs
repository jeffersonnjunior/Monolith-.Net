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
        if (dto is SeatsCreateDto createDto) return ValidateCreateDto(createDto);
        else if (dto is SeatsUpdateDto updateDto) return ValidateUpdateDto(updateDto);
        else if (dto is FilterSeatsById filterSeatsById) return ValidateFilterSeatsById(filterSeatsById);
        else return false;
    }
    
    private bool ValidateCreateDto(SeatsCreateDto dto)
    {
        bool isValid = true;
        isValid &= IsSeatNumberValid(dto.SeatNumber);
        isValid &= IsRowLetterValid(dto.RowLetter);
        isValid &= IsScreenIdValid(dto.ScreenId);
        return isValid;
    }

    private bool ValidateUpdateDto(SeatsUpdateDto dto)
    {
        bool isValid = true;
        isValid &= IsIdValid(dto.Id);
        isValid &= IsSeatNumberValid(dto.SeatNumber);
        isValid &= IsRowLetterValid(dto.RowLetter);
        isValid &= IsScreenIdValid(dto.ScreenId);
        return isValid;
    }
    
    private bool ValidateFilterSeatsById(FilterSeatsById filterSeatsById)
    {
        bool isValid = true;
        isValid &= IsIdValid(filterSeatsById.Id);
        isValid &= IsIncludeValid(filterSeatsById.Includes);
        return isValid;
    }

    private bool IsIncludeValid(string[]? includes)
    {
        if (includes == null || !includes.Any())
        {
            return true;
        }

        var validProperties = typeof(ScreensReadDto).GetProperties(BindingFlags.Public | BindingFlags.Instance)
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

    private bool IsIdValid(Guid id)
    {
        if (id == Guid.Empty)
        {
            _notificationContext.AddNotification("O ID não pode estar vazio.");
            return false;
        }
        return true;
    }

    private bool IsSeatNumberValid(int seatNumber)
    {
        if (seatNumber < 1)
        {
            _notificationContext.AddNotification("O número do assento deve ser maior que zero.");
            return false;
        }
        return true;
    }

    private bool IsRowLetterValid(string rowLetter)
    {
        if (string.IsNullOrWhiteSpace(rowLetter))
        {
            _notificationContext.AddNotification("A letra da fileira não pode estar vazia.");
            return false;
        }
        return true;
    }

    private bool IsScreenIdValid(Guid screenId)
    {
        if (screenId == Guid.Empty)
        {
            _notificationContext.AddNotification("O ScreenId não pode estar vazio.");
            return false;
        }
        return true;
    }
}