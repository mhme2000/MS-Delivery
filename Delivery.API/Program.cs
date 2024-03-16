using Delivery.Application.Interfaces.Customers;
using Delivery.Application.UseCases.Customers;
using Delivery.Consumer;
using Delivery.Domain.Interfaces;
using Delivery.Infrastructure.Contexts;
using Delivery.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddScoped<IDeliveryRepository, DeliveryRepository>();
        builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
        builder.Services.AddScoped<ISendEmailUseCase, SendEmailUseCase>();
        builder.Services.AddScoped<ISearchCustomerByCustomerIdUseCase, SearchCustomerByCustomerIdUseCase>();
        builder.Services.AddDbContext<DeliveryContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        var app = builder.Build();
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        using var scope = app.Services.CreateScope();
        var dataContext = scope.ServiceProvider.GetRequiredService<DeliveryContext>();
        dataContext.Database.Migrate();
        var serviceEmail = scope.ServiceProvider.GetService<ISendEmailUseCase>();
        var serviceCustomer = scope.ServiceProvider.GetService<ISearchCustomerByCustomerIdUseCase>();
        if (serviceEmail != null && serviceCustomer != null)
        {
            Thread threadConsumer = new(() => RabbitConsumer.Consume(serviceEmail, serviceCustomer));
            threadConsumer.Start();
        }
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}

