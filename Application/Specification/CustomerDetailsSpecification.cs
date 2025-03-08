using Application.Dtos;
using Infrastructure.Notifications;

namespace Application.Specification;

public class CustomerDetailsSpecification
{
    private readonly NotificationContext _notificationContext;

    public CustomerDetailsSpecification(NotificationContext notificationContext)
    {
        _notificationContext = notificationContext;
    }

    public bool IsSatisfiedBy(object dto)
    {
        return dto switch
        {
            CustomerDetailsCreateDto createDto => ValidateCreateDto(createDto),
            CustomerDetailsUpdateDto updateDto => ValidateUpdateDto(updateDto),
            _ => false
        };
    }

    public bool ValidateCreateDto(CustomerDetailsCreateDto createDto)
    {
        return ValidateName(createDto.Name)
            && ValidateEmail(createDto.Email)
            && ValidateAge(createDto.Age);
    }

    public bool ValidateUpdateDto(CustomerDetailsUpdateDto updateDto)
    {
        return ValidateId(updateDto.Id)
            && ValidateName(updateDto.Name)
            && ValidateEmail(updateDto.Email)
            && ValidateAge(updateDto.Age);
    }
    private bool ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            _notificationContext.AddNotification("Nome é obrigatório.");
            return false;
        }
        return true;
    }

    private bool ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            _notificationContext.AddNotification("Email é obrigatório.");
            return false;
        }
        else if (!email.Contains("@"))
        {
            _notificationContext.AddNotification("Email é inválido.");
            return false;
        }
        return true;
    }

    private bool ValidateAge(int age)
    {
        if (age < 0)
        {
            _notificationContext.AddNotification("Idade deve ser um número positivo.");
            return false;
        }
        return true;
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
}