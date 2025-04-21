using Application.Dtos;
using Infrastructure.FiltersModel;

namespace Application.Interfaces.IServices;

public interface ICustomerDetailsService
{
    CustomerDetailsReadDto GetById (FilterCustomerDetailsById filterCustomerDetailsById);
    FilterReturn<CustomerDetailsReadDto> GetFilter(FilterCustomerDetailsTable filter);
    CustomerDetailsUpdateDto Add(CustomerDetailsCreateDto customerDetailsCreateDto);
    void Update(CustomerDetailsUpdateDto customerDetailsUpdateDto);
    void Delete(Guid id);
}
