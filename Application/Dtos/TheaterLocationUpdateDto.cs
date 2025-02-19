namespace Application.Dtos;

public class TheaterLocationUpdateDto
{
    public required Guid Id { get; set; }
    public string? Street { get; set; }
    public string? UnitNumber { get; set; }
    public string? PostalCode { get; set; }
}
