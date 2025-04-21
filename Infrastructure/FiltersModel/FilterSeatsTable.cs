namespace Infrastructure.FiltersModel;

public class FilterSeatsTable
{
    public int? SeatNumber { get; set; }
    public string? RowLetter { get; set; }
    public Guid? ScreenId { get; set; }
    public string[]? Includes { get; set; }
    public int PageSize { get; set; } = 10; 
    public int PageNumber { get; set; } = 1; 
}