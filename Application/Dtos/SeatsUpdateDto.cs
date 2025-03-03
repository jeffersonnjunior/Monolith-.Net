namespace Application.Dtos;

public class SeatsUpdateDto
{
    public Guid Id { get; set; }
    public int SeatNumber { get; set; }
    public required string RowLetter { get; set; }
    public Guid ScreenId { get; set; }
}