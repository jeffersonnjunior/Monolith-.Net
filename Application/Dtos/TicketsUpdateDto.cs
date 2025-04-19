namespace Application.Dtos;

public class TicketsUpdateDto
{
    public Guid Id { get; set; }
    public Guid SessionId { get; set; }
    public Guid SeatId { get; set; }
    public Guid CustomerDetailsId { get; set; }
}
