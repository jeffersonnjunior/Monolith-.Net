namespace Application.Dtos;

public class TheaterLocationCreateDto
{
    public required string Street { get; set; }
    public required string UnitNumber { get; set; }
    public required string PostalCode { get; set; }
}