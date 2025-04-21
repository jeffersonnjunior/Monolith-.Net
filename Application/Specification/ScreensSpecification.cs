using Application.Dtos;
using Infrastructure.Notifications;
using System.Reflection;
using Infrastructure.FiltersModel;

namespace Application.Specification;

public class ScreensSpecification
{
    private readonly NotificationContext _notificationContext;

    public ScreensSpecification(NotificationContext notificationContext)
    {
        _notificationContext = notificationContext;
    }

    public bool IsSatisfiedBy(object dto)
    {
        return dto switch
        {
            ScreensCreateDto createDto => ValidateCreateDto(createDto),
            ScreensUpdateDto updateDto => ValidateUpdateDto(updateDto),
            FilterScreensById filterScreensById => ValidateFilterScreensById(filterScreensById),
            _ => false
        };
    }

    private bool ValidateCreateDto(ScreensCreateDto dto)
    {
        bool isValid = true;
        isValid &= ValidateScreenNumber(dto.ScreenNumber);
        isValid &= ValidateSeatingCapacity(dto.SeatingCapacity);
        return isValid;
    }

    private bool ValidateUpdateDto(ScreensUpdateDto dto)
    {
        bool isValid = true;
        isValid &= ValidateId(dto.Id);
        isValid &= ValidateScreenNumber(dto.ScreenNumber);
        isValid &= ValidateSeatingCapacity(dto.SeatingCapacity);
        return isValid;
    }

    private bool ValidateFilterScreensById(FilterScreensById filterScreensById)
    {
        bool isValid = true;
        isValid &= ValidateId(filterScreensById.Id);
        isValid &= ValidateIncludes(filterScreensById.Includes);
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

    private bool ValidateIncludes(IEnumerable<string> includes)
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
    
    private bool ValidateScreenNumber(string screenNumber)
    {
        if (string.IsNullOrWhiteSpace(screenNumber))
        {
            _notificationContext.AddNotification("O número da sala não pode estar vazio.");
            return false;
        }
        return true;
    }

    private bool ValidateSeatingCapacity(int seatingCapacity)
    {
        if (seatingCapacity <= 0)
        {
            _notificationContext.AddNotification("A capacidade de assentos deve ser maior que zero.");
            return false;
        }
        return true;
    }
}