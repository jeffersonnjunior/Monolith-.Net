using Domain.Enums;

namespace Domain.Entities;

public class Films
{
    public Guid Id { get; set; } 
    public required string Name { get; set; }
    public int Duration { get; set; }
    public int AgeRange { get; set; }
    public FilmAudioOption FilmAudioOption { get; set; }
    public FilmFormat FilmFormat { get; set; }
    public FilmGenres FilmGenres { get; set; }
    public virtual ICollection<Sessions> Sessions { get; set; }
}