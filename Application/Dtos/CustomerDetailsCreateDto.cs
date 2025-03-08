namespace Application.Dtos;

public class CustomerDetailsCreateDto
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public int Age { get; set; }
}