namespace Application.Dtos;

public class TicketsReadDto
{
    public Guid Id { get; set; }
    public Guid SessionId { get; set; }
    public Guid SeatId { get; set; }
    public Guid ClientId { get; set; }

    public virtual SessionsReadDto Session { get; set; }
    public virtual SeatsReadDto Seat { get; set; }
    public virtual CustomerDetailsReadDto CustomerDetails { get; set; }
}
