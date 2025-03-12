using Application.Dtos;
using Application.Interfaces.IServices;
using Application.Specification;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces.IRepositories;
using Infrastructure.Notifications;
using Infrastructure.Utilities.FiltersModel;

namespace Application.Services;

public class CustomerDetailsService : ICustomerDetailsService
{
    private readonly ICustomerDetailsRepository _customerDetailsRepository;
    private readonly IMapper _mapper;
    private readonly NotificationContext _notificationContext;
    private readonly CustomerDetailsSpecification _customerDetailsSpecification;
    
    public CustomerDetailsService(ICustomerDetailsRepository customerDetailsRepository, IMapper mapper, NotificationContext notifierContext)
    {
        _customerDetailsRepository = customerDetailsRepository;
        _mapper = mapper;
        _notificationContext = notifierContext;
        _customerDetailsSpecification = new CustomerDetailsSpecification(notifierContext);
    }

    public CustomerDetailsReadDto GetById(FilterCustomerDetailsById filterCustomerDetailsById)
    {
        return _mapper.Map<CustomerDetailsReadDto>(_customerDetailsRepository.GetByElement(new FilterByItem { Field = "Id", Value = filterCustomerDetailsById.Id, Key = "Equal", Includes = filterCustomerDetailsById.Includes }));
    }

    public FilterReturn<CustomerDetailsReadDto> GetFilter(FilterCustomerDetailsTable filter)
    {
        var filterResult = _customerDetailsRepository.GetFilter(filter);
        return new FilterReturn<CustomerDetailsReadDto>
        {
            TotalRegister = filterResult.TotalRegister,
            TotalRegisterFilter = filterResult.TotalRegisterFilter,
            TotalPages = filterResult.TotalPages,
            ItensList = _mapper.Map<IEnumerable<CustomerDetailsReadDto>>(filterResult.ItensList)
        };
    }

    public CustomerDetailsUpdateDto Add(CustomerDetailsCreateDto customerDetailsCreateDto)
    {
        CustomerDetailsUpdateDto customerDetailsUpdateDto = null;

        if (!_customerDetailsSpecification.IsSatisfiedBy(customerDetailsCreateDto)) return customerDetailsUpdateDto;
        
        if(!_customerDetailsRepository.ValidateInput(customerDetailsCreateDto, false)) return customerDetailsUpdateDto;
        
        CustomerDetails customerDetails = _mapper.Map<CustomerDetails>(customerDetailsCreateDto);
        
        customerDetailsUpdateDto = _mapper.Map<CustomerDetailsUpdateDto>(_customerDetailsRepository.Add(customerDetails));

        return customerDetailsUpdateDto;
    }

    public void Update(CustomerDetailsUpdateDto customerDetailsUpdateDto)
    {
        if(!_customerDetailsSpecification.IsSatisfiedBy(customerDetailsUpdateDto)) return;
        
        var existingCustomerDetails = _customerDetailsRepository.GetByElement(new FilterByItem { Field = "Id", Value = customerDetailsUpdateDto.Id, Key = "Equal" });
        
        if(!_customerDetailsRepository.ValidateInput(customerDetailsUpdateDto, false, existingCustomerDetails)) return;
        
        var customerDetails = _mapper.Map<CustomerDetails>(customerDetailsUpdateDto);
        
        _customerDetailsRepository.Update(customerDetails);
    }

    public void Delete(Guid id)
    {
        CustomerDetails existingCustomerDetails = _customerDetailsRepository.GetByElement(new FilterByItem { Field = "Id", Value = id, Key = "Equal" });

        if (existingCustomerDetails is null) return;

        _customerDetailsRepository.Delete(existingCustomerDetails);        
    }
}
