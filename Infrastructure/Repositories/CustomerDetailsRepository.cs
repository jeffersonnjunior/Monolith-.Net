using Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Interfaces.IRepositories;
using Infrastructure.Notifications;
using Infrastructure.FiltersModel;

namespace Infrastructure.Repositories;

public class CustomerDetailsRepository : BaseRepository<CustomerDetails>, ICustomerDetailsRepository
{
    private readonly NotificationContext _notificationContext;
    public CustomerDetailsRepository(AppDbContext context, IUnitOfWork unitOfWork, NotificationContext notificationContext ) : base(context, unitOfWork, notificationContext)
    {
        _notificationContext = notificationContext;
    }

    public CustomerDetails GetByElement(FilterByItem filterByItem)
    {
        (CustomerDetails customerDetails, bool validadeIncludes) = GetElementEqual(filterByItem);

        if (validadeIncludes) return customerDetails;

        if (filterByItem.Field == "Id" && customerDetails is null) _notificationContext.AddNotification("Usuário não registrado!");

        return customerDetails;
    }

    public FilterReturn<CustomerDetails> GetFilter(FilterCustomerDetailsTable filter)
    {
        var filters = new Dictionary<string, string>();

        if (!string.IsNullOrEmpty(filter.NameContains))
            filters.Add(nameof(filter.NameContains), filter.NameContains);

        if (!string.IsNullOrEmpty(filter.EmailContains))
            filters.Add(nameof(filter.EmailContains), filter.EmailContains);
        
        if (filter.AgeEqual.HasValue)
            filters.Add(nameof(filter.AgeEqual), filter.AgeEqual.Value.ToString());

        (var result, bool validadeIncludes) = GetFilters(filters, filter.PageSize, filter.PageNumber, filter.Includes);

        return result;
    }

    public bool ValidateInput(object dto, bool isUpdate, CustomerDetails existingCustomerDetails = null)
    {
        var isValid = true;
        dynamic customerDetailsDto = dto;
        
        if(!IsEmailInUse(existingCustomerDetails, customerDetailsDto.Email)) isValid = false;
        
        return isValid;
    }
    
    private bool IsEmailInUse(object? existingCustomerDetails, string email)
    {
        if ((existingCustomerDetails == null || ((dynamic)existingCustomerDetails).Email != email) &&
            GetByElement(new FilterByItem { Field = "Email", Value = email, Key = "Equal" }) is not null)
        {
            _notificationContext.AddNotification("Esse email já está cadastrado.");
            return false;
        }
        return true;
    }
}