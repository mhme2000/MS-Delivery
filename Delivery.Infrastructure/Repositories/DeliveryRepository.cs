using Delivery.Domain.DTOs;
using Delivery.Domain.Entities;
using Delivery.Domain.Interfaces;
using Delivery.Infrastructure.Contexts;
using System.Net;
using System.Net.Mail;

namespace Delivery.Infrastructure.Repositories;

public class DeliveryRepository(DeliveryContext context) : IDeliveryRepository
{
    private readonly DeliveryContext _context = context;
    private static readonly SmtpClient _smtpClient = new("sandbox.smtp.mailtrap.io", 2525)
    {
        Credentials = new NetworkCredential("88f6b8f903bc5c", "6473d74f9edd5a"),
        EnableSsl = true
    };

    public void RegisterLog(SendEmailLog model)
    {
        _context.SendEmailLog.Add(model);
        _context.SaveChanges();
    }

    public void SendEmail(SendEmailDTO dto)
    {
        _smtpClient.Send("from@example.com", dto.Email, dto.Subject, dto.Content);
    }
}
