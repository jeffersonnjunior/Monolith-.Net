using Domain.Entities;
using Infrastructure.Context;
using Infrastructure.Interfaces.IRepositories;
using Infrastructure.Notifications;

namespace Infrastructure.Repositories;

public class CustomerDetailsRepository : BaseRepository<CustomerDetails>, ICustomerDetailsRepository
{
    public CustomerDetailsRepository(AppDbContext context, IUnitOfWork unitOfWork, NotificationContext notificationContext ) : base(context, unitOfWork, notificationContext)
    {
    }
}
