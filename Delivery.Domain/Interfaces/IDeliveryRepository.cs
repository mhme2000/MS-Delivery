using Delivery.Domain.DTOs;
using Delivery.Domain.Entities;

namespace Delivery.Domain.Interfaces;

public interface IDeliveryRepository
{
    void SendEmail(SendEmailDTO dto);
    void RegisterLog(SendEmailLog model);
}