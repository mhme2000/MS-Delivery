using Delivery.Application.Interfaces.Customers;
using Delivery.Domain.DTOs;
using Delivery.Domain.Entities;
using Delivery.Domain.Interfaces;

namespace Delivery.Application.UseCases.Customers;

public class SendEmailUseCase(IDeliveryRepository deliveryRepository) : ISendEmailUseCase
{
    private readonly IDeliveryRepository _deliveryRepository = deliveryRepository;

    public object Execute(SendEmailDTO dto)
    {
        _deliveryRepository.SendEmail(dto);
        var sendEmailLogModel = new SendEmailLog(dto.Email, dto.Content, dto.Subject);
        _deliveryRepository.RegisterLog(sendEmailLogModel);
        return new object { };
    }
}
