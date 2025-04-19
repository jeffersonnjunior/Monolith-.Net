using Application.Dtos;
using Domain.Entities;

namespace Application.Interfaces.IFactory;

public interface ICustomerDetailsFactory
{
    CustomerDetails CreateCustomerDetails();
    CustomerDetailsCreateDto CreateCustomerDetailsCreateDto();
    CustomerDetailsReadDto CreateCustomerDetailsReadDto();
    CustomerDetailsUpdateDto CreateCustomerDetailsUpdateDto();
    CustomerDetails MapToCustomerDetails(CustomerDetailsCreateDto dto);
    CustomerDetailsCreateDto MapToCustomerDetailsCreateDto(CustomerDetails entity);
    CustomerDetailsReadDto MapToCustomerDetailsReadDto(CustomerDetails entity);
    CustomerDetailsUpdateDto MapToCustomerDetailsUpdateDto(CustomerDetails entity);
    CustomerDetails MapToCustomerDetailsFromUpdateDto(CustomerDetailsUpdateDto dto);
}