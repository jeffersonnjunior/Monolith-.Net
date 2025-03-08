using Domain.Entities;
using Infrastructure.Utilities.FiltersModel;

namespace Infrastructure.Interfaces.IRepositories;

public interface ICustomerDetailsRepository : IBaseRepository<CustomerDetails>
{
    CustomerDetails GetByElement(FilterByItem filterByItem);
    FilterReturn<CustomerDetails> GetFilter(FilterCustomerDetailsTable filter);
    bool ValidateInput(object dto, bool isUpdate, CustomerDetails existingCustomerDetails = null);
}
