namespace Infrastructure.Utilities.FiltersModel;

public class FilterMovieTheatersTable
{
    public string? Name { get; set; }
    public Guid? TheaterLocationId { get; set; }
    public string[]? Includes { get; set; }
    public int PageSize { get; set; } = 10; 
    public int PageNumber { get; set; } = 1; 
}