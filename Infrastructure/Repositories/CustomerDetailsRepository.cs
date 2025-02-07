using Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Interfaces;

namespace Infrastructure.Repositories;

public class CustomerDetailsRepository : BaseRepository<CustomerDetails>, ICustomerDetailsRepository
{
    public CustomerDetailsRepository(AppDbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
    {
    }
}
