using Application.Dtos;
using Application.Interfaces.IFactories;
using Domain.Entities;

namespace Application.Factories;

public class CustomerDetailsFactory : ICustomerDetailsFactory
{
    public CustomerDetails CreateCustomerDetails()
    {
        return new CustomerDetails
        {
            Id = Guid.NewGuid(),
            Name = string.Empty,
            Email = string.Empty,
            Age = 0
        };
    }

    public CustomerDetailsCreateDto CreateCustomerDetailsCreateDto()
    {
        return new CustomerDetailsCreateDto
        {
            Name = string.Empty,
            Email = string.Empty,
            Age = 0
        };
    }

    public CustomerDetailsReadDto CreateCustomerDetailsReadDto()
    {
        return new CustomerDetailsReadDto
        {
            Id = Guid.NewGuid(),
            Name = string.Empty,
            Email = string.Empty,
            Age = 0
        };
    }

    public CustomerDetailsUpdateDto CreateCustomerDetailsUpdateDto()
    {
        return new CustomerDetailsUpdateDto
        {
            Id = Guid.NewGuid(),
            Name = string.Empty,
            Email = string.Empty,
            Age = 0
        };
    }

    public CustomerDetails MapToCustomerDetails(CustomerDetailsCreateDto dto)
    {
        return new CustomerDetails
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Email = dto.Email,
            Age = dto.Age
        };
    }

    public CustomerDetailsCreateDto MapToCustomerDetailsCreateDto(CustomerDetails entity)
    {
        return new CustomerDetailsCreateDto
        {
            Name = entity.Name,
            Email = entity.Email,
            Age = entity.Age
        };
    }

    public CustomerDetailsReadDto MapToCustomerDetailsReadDto(CustomerDetails entity)
    {
        return new CustomerDetailsReadDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Email = entity.Email,
            Age = entity.Age
        };
    }

    public CustomerDetailsUpdateDto MapToCustomerDetailsUpdateDto(CustomerDetails entity)
    {
        return new CustomerDetailsUpdateDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Email = entity.Email,
            Age = entity.Age
        };
    }

    public CustomerDetails MapToCustomerDetailsFromUpdateDto(CustomerDetailsUpdateDto dto)
    {
        return new CustomerDetails
        {
            Id = dto.Id,
            Name = dto.Name,
            Email = dto.Email,
            Age = dto.Age
        };
    }
}