namespace Domain.Entities;

public class MovieTheaters
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public Guid AddressId { get; set; }
    public virtual TheaterLocation TheaterLocation { get; set; }
    public virtual ICollection<Screens> Screens { get; set; }
}