namespace Application.Dtos;

public class TicketsCreateDto
{
    public Guid SessionId { get; set; }
    public Guid SeatId { get; set; }
    public Guid ClientId { get; set; }
}
