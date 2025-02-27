namespace Application.Dtos;

public class MovieTheatersCreateDto
{
    public required string Name { get; set; }
    public required Guid TheaterLocationId { get; set; }
}