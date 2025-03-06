using Domain.Enums;

namespace Infrastructure.Utilities.FiltersModel;

public class FilterFilmsTable
{
    public string? NameContains { get; set; }
    public int? DurationEqual { get; set; }
    public int? AgeRangeEqual { get; set; }
    public FilmGenres? FilmGenresEqual { get; set; }
    public string[]? Includes { get; set; }
    public int PageSize { get; set; } = 10; 
    public int PageNumber { get; set; } = 1; 
}