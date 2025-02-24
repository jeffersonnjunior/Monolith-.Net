namespace Infrastructure.Utilities.FiltersModel;

public class FilterTheaterLocation
{
    public string? StreetContains { get; set; }
    public string? UnitNumberContains { get; set; }
    public string? PostalCodeContains { get; set; }
    public string[]? Includes { get; set; }
    public int PageSize { get; set; } = 10; 
    public int PageNumber { get; set; } = 1; 
}