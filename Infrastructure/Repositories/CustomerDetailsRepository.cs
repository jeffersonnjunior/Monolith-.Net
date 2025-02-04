using Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class CustomerDetailsRepository : BaseRepository<CustomerDetails>, ICustomerDetailsRepository
{
    public CustomerDetailsRepository(DbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
    {
    }
}
