using Domain.Entities;

namespace Application.Dtos;

public class MovieTheatersReadDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public Guid TheaterLocationId { get; set; }

    public virtual TheaterLocation TheaterLocation { get; set; }
    public virtual ICollection<ScreensDto> Screens { get; set; }
}
