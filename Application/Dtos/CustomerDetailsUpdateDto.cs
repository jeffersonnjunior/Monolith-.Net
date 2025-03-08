namespace Application.Dtos;

public class CustomerDetailsUpdateDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public int Age { get; set; }
}
