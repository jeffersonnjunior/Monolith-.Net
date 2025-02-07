namespace Application.Dtos;

public class TheaterLocationDto
{
    public Guid Id { get; set; }
    public required string Street { get; set; }
    public required string UnitNumber { get; set; }
    public required string PostalCode { get; set; }
}
