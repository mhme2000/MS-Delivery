using Delivery.Domain.Interfaces;
using System.Net.Http.Json;

namespace Delivery.Infrastructure.Repositories;

public class CustomerRepository() : ICustomerRepository
{
    private record SearchResponse
    {
        public string Email { get; set; } = null!;
        public string Document { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Phone { get; set; }
        public string? Address { get; set; }
    }
    private static readonly HttpClient _httpClient = new HttpClient();
    public async Task<string> SearchCustomerByCustomerIdAsync(Guid customerId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"http://http-alb-631019594.us-east-1.elb.amazonaws.com/customers/{customerId}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadFromJsonAsync<SearchResponse>();
            return content != null ? content.Email : "try@mock.com";
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return "catch@mock.com";
        }
    }
}
