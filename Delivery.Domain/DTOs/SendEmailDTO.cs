using System.ComponentModel.DataAnnotations;

namespace Delivery.Domain.DTOs;

public class SendEmailDTO
{
    [Required]
    public string Email { get; set; } = null!;
    [Required]
    public string Subject { get; set; } = null!;
    [Required]
    public string Content { get; set; } = null!;
}