using Domain.Enums;

namespace Application.Dtos;

public class SessionsDto
{
    public Guid Id { get; set; }
    public DateTime SessionTime { get; set; }
    public Guid FilmId { get; set; }
    public FilmAudioOption FilmAudioOption { get; set; }
    public FilmFormat FilmFormat { get; set; }

    public virtual FilmsDto Film { get; set; }
    public virtual ICollection<TicketsDto> Tickets { get; set; }
}
