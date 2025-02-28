namespace Application.Dtos;

public class SeatsDto
{
    public Guid Id { get; set; }
    public int SeatNumber { get; set; }
    public required string RowLetter { get; set; }
    public Guid ScreenId { get; set; }

    public virtual ScreensReadDto ScreenRead { get; set; }
    public virtual ICollection<TicketsDto> Tickets { get; set; }
}
