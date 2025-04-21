using Domain.Entities;

namespace Infrastructure.FiltersModel;

public class FilterScreensTable
{
    public string ScreenNumberContains { get; set; }
    public int? SeatingCapacityEqual { get; set; }
    public  Guid? MovieTheaterIdEqual { get; set; }
    public string[]? Includes { get; set; }
    public int PageSize { get; set; } = 10; 
    public int PageNumber { get; set; } = 1; 
}