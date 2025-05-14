using Application.Dtos;
using Application.Interfaces.IFactories;
using Application.Interfaces.IServices;
using Application.Specification;
using Infrastructure.Interfaces.IRepositories;
using Infrastructure.Notifications;
using Infrastructure.FiltersModel;
using Infrastructure.Interfaces.ICache.IServices;

namespace Application.Services;

public class CustomerDetailsService : ICustomerDetailsService
{
    private readonly ICustomerDetailsRepository _customerDetailsRepository;
    private readonly ICustomerDetailsFactory _customerDetailsFactory;
    private readonly ICacheService _cacheService;
    private readonly NotificationContext _notificationContext;
    private readonly CustomerDetailsSpecification _customerDetailsSpecification;

    public CustomerDetailsService(
        ICustomerDetailsRepository customerDetailsRepository,
        ICustomerDetailsFactory customerDetailsFactory,
        ICacheService cacheService,
        NotificationContext notifierContext)
    {
        _customerDetailsRepository = customerDetailsRepository;
        _customerDetailsFactory = customerDetailsFactory;
        _cacheService = cacheService;
        _customerDetailsSpecification = new CustomerDetailsSpecification(notifierContext);
    }

    public CustomerDetailsReadDto GetById(FilterCustomerDetailsById filterCustomerDetailsById)
    {
        string cacheKey = $"CustomerDetails:Id:{filterCustomerDetailsById.Id}";
        var cached = _cacheService.Get<CustomerDetailsReadDto>(cacheKey);
        
        if (cached != null) return cached;

        var customerDetails = _customerDetailsRepository.GetByElement(new FilterByItem
        {
            Field = "Id",
            Value = filterCustomerDetailsById.Id,
            Key = "Equal",
            Includes = filterCustomerDetailsById.Includes
        });

        var dto = _customerDetailsFactory.MapToCustomerDetailsReadDto(customerDetails);
        _cacheService.Set(cacheKey, dto, TimeSpan.FromMinutes(10)); 
        return dto;
    }

    public FilterReturn<CustomerDetailsReadDto> GetFilter(FilterCustomerDetailsTable filter)
    {
        string cacheKey = $"CustomerDetails:Filter:{filter.GetHashCode()}";
        var cached = _cacheService.Get<FilterReturn<CustomerDetailsReadDto>>(cacheKey);
        
        if (cached != null) return cached;

        var filterResult = _customerDetailsRepository.GetFilter(filter);
        var result = new FilterReturn<CustomerDetailsReadDto>
        {
            TotalRegister = filterResult.TotalRegister,
            TotalRegisterFilter = filterResult.TotalRegisterFilter,
            TotalPages = filterResult.TotalPages,
            ItensList = filterResult.ItensList.Select(_customerDetailsFactory.MapToCustomerDetailsReadDto)
        };
        _cacheService.Set(cacheKey, result, TimeSpan.FromMinutes(10));
        return result;
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

        string cacheKey = $"CustomerDetails:Id:{customerDetailsUpdateDto.Id}";
        _cacheService.Remove(cacheKey);
        _cacheService.RemoveByPrefix("CustomerDetails:Filter:");
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

        string cacheKey = $"CustomerDetails:Id:{id}";
        _cacheService.Remove(cacheKey);
        _cacheService.RemoveByPrefix("CustomerDetails:Filter:");
    }
}