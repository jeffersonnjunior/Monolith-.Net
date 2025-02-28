namespace Application.Dtos;

public class ScreensUpdateDto
{
    public Guid Id { get; set; }
    public required string ScreenNumber { get; set; }
    public int SeatingCapacity { get; set; }
    public required Guid MovieTheaterId { get; set; }
}