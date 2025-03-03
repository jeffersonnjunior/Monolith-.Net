using Application.Dtos;
using Application.Interfaces.ISpecification;
using Infrastructure.Notifications;
using System.Reflection;
using Infrastructure.Utilities.FiltersModel;

namespace Application.Specification;

public class ScreensSpecification<T> : ISpecificationBase<T>
    where T : class
{
    private readonly NotificationContext _notificationContext;

    public ScreensSpecification(NotificationContext notificationContext)
    {
        _notificationContext = notificationContext;
    }

    public bool IsSatisfiedBy(T dto)
    {
        if (dto is ScreensCreateDto createDto) return ValidateCreateDto(createDto);
        else if (dto is ScreensUpdateDto updateDto) return ValidateUpdateDto(updateDto);
        else if (dto is FilterScreensById filterScreensById) return ValidateFilterScreensById(filterScreensById);
        else return false;
    }

    private bool ValidateCreateDto(ScreensCreateDto dto)
    {
        bool isValid = true;
        isValid &= IsScreenNumberValid(dto.ScreenNumber);
        isValid &= IsSeatingCapacityValid(dto.SeatingCapacity);
        return isValid;
    }

    private bool ValidateUpdateDto(ScreensUpdateDto dto)
    {
        bool isValid = true;
        isValid &= IsIdValid(dto.Id);
        isValid &= IsScreenNumberValid(dto.ScreenNumber);
        isValid &= IsSeatingCapacityValid(dto.SeatingCapacity);
        return isValid;
    }

    private bool ValidateFilterScreensById(FilterScreensById filterScreensById)
    {
        bool isValid = true;
        isValid &= IsIdValid(filterScreensById.Id);
        isValid &= IncludesValid(filterScreensById.Includes);
        return isValid;
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

    private bool IsScreenNumberValid(string screenNumber)
    {
        if (string.IsNullOrWhiteSpace(screenNumber))
        {
            _notificationContext.AddNotification("O número da sala não pode estar vazio.");
            return false;
        }
        return true;
    }

    private bool IsSeatingCapacityValid(int seatingCapacity)
    {
        if (seatingCapacity <= 0)
        {
            _notificationContext.AddNotification("A capacidade de assentos deve ser maior que zero.");
            return false;
        }
        return true;
    }
    private bool IncludesValid(IEnumerable<string> includes)
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
}