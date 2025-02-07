namespace Application.Dtos;

public class CustomerDetailsDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public int Age { get; set; }

    public virtual ICollection<TicketsDto> Tickets { get; set; }
}
