namespace Application.Dtos;

public class MovieTheatersUpdateDto
{
    public required Guid Id { get; set; } 
    public required string Name { get; set; }
    public required Guid TheaterLocationId { get; set; }
}