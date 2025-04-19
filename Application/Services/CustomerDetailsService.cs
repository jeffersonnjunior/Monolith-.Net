using Application.Dtos;
using Application.Interfaces.IFactory;
using Application.Interfaces.IServices;
using Application.Specification;
using Domain.Entities;
using Infrastructure.Interfaces.IRepositories;
using Infrastructure.Notifications;
using Infrastructure.Utilities.FiltersModel;

namespace Application.Services;

public class CustomerDetailsService : ICustomerDetailsService
{
    private readonly ICustomerDetailsRepository _customerDetailsRepository;
    private readonly ICustomerDetailsFactory _customerDetailsFactory;
    private readonly NotificationContext _notificationContext;
    private readonly CustomerDetailsSpecification _customerDetailsSpecification;
    
    public CustomerDetailsService(
        ICustomerDetailsRepository customerDetailsRepository, 
        ICustomerDetailsFactory customerDetailsFactory, 
        NotificationContext notifierContext)
    {
        _customerDetailsRepository = customerDetailsRepository;
        _customerDetailsFactory = customerDetailsFactory;
        _notificationContext = notifierContext;
        _customerDetailsSpecification = new CustomerDetailsSpecification(notifierContext);
    }

    public CustomerDetailsReadDto GetById(FilterCustomerDetailsById filterCustomerDetailsById)
    {
        var customerDetails = _customerDetailsRepository.GetByElement(new FilterByItem 
        { 
            Field = "Id", 
            Value = filterCustomerDetailsById.Id, 
            Key = "Equal", 
            Includes = filterCustomerDetailsById.Includes 
        });

        return _customerDetailsFactory.MapToCustomerDetailsReadDto(customerDetails);
    }

    public FilterReturn<CustomerDetailsReadDto> GetFilter(FilterCustomerDetailsTable filter)
    {
        var filterResult = _customerDetailsRepository.GetFilter(filter);
        return new FilterReturn<CustomerDetailsReadDto>
        {
            TotalRegister = filterResult.TotalRegister,
            TotalRegisterFilter = filterResult.TotalRegisterFilter,
            TotalPages = filterResult.TotalPages,
            ItensList = filterResult.ItensList.Select(_customerDetailsFactory.MapToCustomerDetailsReadDto)
        };
    }

    public CustomerDetailsUpdateDto Add(CustomerDetailsCreateDto customerDetailsCreateDto)
    {
        CustomerDetailsUpdateDto customerDetailsUpdateDto = null;

        if (!_customerDetailsSpecification.IsSatisfiedBy(customerDetailsCreateDto)) return customerDetailsUpdateDto;
        
        if (!_customerDetailsRepository.ValidateInput(customerDetailsCreateDto, false)) return customerDetailsUpdateDto;
        
        var customerDetails = _customerDetailsFactory.MapToCustomerDetails(customerDetailsCreateDto);
        
        var addedCustomerDetails = _customerDetailsRepository.Add(customerDetails);
        
        customerDetailsUpdateDto = _customerDetailsFactory.MapToCustomerDetailsUpdateDto(addedCustomerDetails);

        return customerDetailsUpdateDto;
    }

    public void Update(CustomerDetailsUpdateDto customerDetailsUpdateDto)
    {
        if (!_customerDetailsSpecification.IsSatisfiedBy(customerDetailsUpdateDto)) return;
        
        var existingCustomerDetails = _customerDetailsRepository.GetByElement(new FilterByItem 
        { 
            Field = "Id", 
            Value = customerDetailsUpdateDto.Id, 
            Key = "Equal" 
        });
        
        if (!_customerDetailsRepository.ValidateInput(customerDetailsUpdateDto, false, existingCustomerDetails)) return;
        
        var customerDetails = _customerDetailsFactory.MapToCustomerDetailsFromUpdateDto(customerDetailsUpdateDto);
        
        _customerDetailsRepository.Update(customerDetails);
    }

    public void Delete(Guid id)
    {
        var existingCustomerDetails = _customerDetailsRepository.GetByElement(new FilterByItem 
        { 
            Field = "Id", 
            Value = id, 
            Key = "Equal" 
        });

        if (existingCustomerDetails is null) return;

        _customerDetailsRepository.Delete(existingCustomerDetails);        
    }
}