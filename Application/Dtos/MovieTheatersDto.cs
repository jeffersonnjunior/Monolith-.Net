using Domain.Entities;

namespace Application.Dtos;

public class MovieTheatersDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public Guid AddressId { get; set; }

    public virtual TheaterLocationDto TheaterLocation { get; set; }
    public virtual ICollection<ScreensDto> Screens { get; set; }
}
