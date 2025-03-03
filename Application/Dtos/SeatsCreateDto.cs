namespace Application.Dtos;

public class SeatsCreateDto
{
    public int SeatNumber { get; set; }
    public required string RowLetter { get; set; }
    public Guid ScreenId { get; set; }
}