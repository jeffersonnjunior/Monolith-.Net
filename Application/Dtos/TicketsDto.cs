namespace Application.Dtos;

public class TicketsDto
{
    public Guid Id { get; set; }
    public Guid SessionId { get; set; }
    public Guid SeatId { get; set; }
    public Guid ClientId { get; set; }

    public virtual SessionsDto Session { get; set; }
    public virtual SeatsReadDto SeatRead { get; set; }
    public virtual CustomerDetailsDto CustomerDetails { get; set; }
}
