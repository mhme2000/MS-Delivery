namespace Delivery.Domain.Interfaces;

public interface ICustomerRepository
{
    Task<string> SearchCustomerByCustomerIdAsync(Guid customerId);
}