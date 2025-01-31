namespace Domain.Entities;

public class CustomerDetails
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public int Age { get; set; }
    public virtual ICollection<Tickets> Tickets { get; set; }
}