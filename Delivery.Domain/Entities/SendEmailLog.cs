namespace Delivery.Domain.Entities;

public class SendEmailLog(string email, string content, string subject) : Entity
{
    public string Email { get; set; } = email;
    public string Subject { get; set; } = subject;
    public string Content { get; set; } = content;
}