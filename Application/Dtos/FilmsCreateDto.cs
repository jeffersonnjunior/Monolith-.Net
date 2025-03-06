using Domain.Enums;

namespace Application.Dtos;

public class FilmsCreateDto
{
    public required string Name { get; set; }
    public int Duration { get; set; }
    public int AgeRange { get; set; }
    public FilmGenres FilmGenres { get; set; }
}
