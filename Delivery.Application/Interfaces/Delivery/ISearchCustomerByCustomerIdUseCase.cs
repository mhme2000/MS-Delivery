namespace Delivery.Application.Interfaces.Customers;

public interface ISearchCustomerByCustomerIdUseCase : IUseCase<Task<string>, Guid>
{
}