namespace Infrastructure.FiltersModel;

public class FilterTicketsTable
{
    public Guid? SessionIdEqual { get; set; }
    public Guid? SeatIdEqual { get; set; }
    public Guid? ClientIdEqual { get; set; }
    public string[]? Includes { get; set; }
    public int PageSize { get; set; } = 10; 
    public int PageNumber { get; set; } = 1; 
}