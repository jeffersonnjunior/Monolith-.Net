namespace Domain.Entities;

public class Seats
{
    public Guid Id { get; set; }
    public int SeatNumber { get; set; }
    public required string RowLetter { get; set; }
    public Guid ScreenId { get; set; }
    public virtual Screens Screen { get; set; }
    public virtual ICollection<Tickets> Tickets { get; set; }
}