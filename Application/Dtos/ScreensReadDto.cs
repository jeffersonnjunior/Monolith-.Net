namespace Application.Dtos;

public class ScreensReadDto
{
    public Guid Id { get; set; }
    public required string ScreenNumber { get; set; }
    public int SeatingCapacity { get; set; }
    public required Guid MovieTheaterId { get; set; }
    
    public virtual ICollection<SeatsDto> Seats { get; set; }
}
