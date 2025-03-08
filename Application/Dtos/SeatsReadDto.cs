namespace Application.Dtos;

public class SeatsReadDto
{
    public Guid Id { get; set; }
    public int SeatNumber { get; set; }
    public required string RowLetter { get; set; }
    public Guid ScreenId { get; set; }

    public virtual ICollection<TicketsReadDto> Tickets { get; set; }
}