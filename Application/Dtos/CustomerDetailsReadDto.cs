namespace Application.Dtos;

public class CustomerDetailsReadDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public int Age { get; set; }

    public virtual ICollection<TicketsReadDto> Tickets { get; set; }
}
