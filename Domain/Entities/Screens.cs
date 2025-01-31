namespace Domain.Entities;

public class Screens
{
    public Guid Id { get; set; }
    public required string ScreenNumber { get; set; }
    public int SeatingCapacity { get; set; }
    public required Guid MovieTheaterId { get; set; }
    public virtual MovieTheaters MovieTheater { get; set; }
    public virtual ICollection<Seats> Seats { get; set; }
}