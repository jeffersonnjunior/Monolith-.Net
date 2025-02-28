namespace Application.Dtos;

public class ScreensCreateDto
{
    public required string ScreenNumber { get; set; }
    public int SeatingCapacity { get; set; }
    public required Guid MovieTheaterId { get; set; }
}