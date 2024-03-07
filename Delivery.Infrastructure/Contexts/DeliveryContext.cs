using Microsoft.EntityFrameworkCore;
using Delivery.Domain.Entities;

namespace Delivery.Infrastructure.Contexts;

public class DeliveryContext(DbContextOptions<DeliveryContext> options) : DbContext(options)
{
    public DbSet<SendEmailLog> SendEmailLog { get; set; } = null!;
}