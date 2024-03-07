namespace Delivery.Domain.Entities;

public class Entity
{
    public Guid Id { get; protected set; } = Guid.NewGuid();
    public DateTime CreatedDate { get; protected set; } = DateTime.Now;
}
