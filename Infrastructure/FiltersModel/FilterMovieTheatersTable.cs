namespace Infrastructure.Utilities.FiltersModel;

public class FilterMovieTheatersTable
{
    public string? NameContains { get; set; }
    public Guid? TheaterLocationIdEqual { get; set; }
    public string[]? Includes { get; set; }
    public int PageSize { get; set; } = 10; 
    public int PageNumber { get; set; } = 1; 
}