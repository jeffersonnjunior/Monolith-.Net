using Domain.Entities;

namespace Application.Dtos;

public class MovieTheatersReadDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public Guid TheaterLocationId { get; set; }
    
    public virtual ICollection<ScreensReadDto> Screens { get; set; }
}
