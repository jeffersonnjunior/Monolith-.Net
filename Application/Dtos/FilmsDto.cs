using Domain.Enums;

namespace Application.Dtos;

public class FilmsDto
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public int Duration { get; set; }
    public int AgeRange { get; set; }
    public FilmGenres FilmGenres { get; set; }

    public virtual ICollection<SessionsDto> Sessions { get; set; }
}
