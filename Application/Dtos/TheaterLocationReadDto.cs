namespace Application.Dtos;

public class TheaterLocationReadDto
{
    public Guid Id { get; set; }
    public string Street { get; set; }
    public string UnitNumber { get; set; }
    public string PostalCode { get; set; }
}