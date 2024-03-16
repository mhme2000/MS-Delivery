using Delivery.Application.Interfaces.Customers;
using Delivery.Domain.Interfaces;

namespace Delivery.Application.UseCases.Customers;

public class SearchCustomerByCustomerIdUseCase(ICustomerRepository customerDelivery) : ISearchCustomerByCustomerIdUseCase
{
    private readonly ICustomerRepository _customerDelivery = customerDelivery;

    public async Task<string> Execute(Guid customerId)
    {
        return await _customerDelivery.SearchCustomerByCustomerIdAsync(customerId);
    }
}
