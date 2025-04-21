using Domain.Enums;

namespace Infrastructure.Utilities.FiltersModel;

public class FilterSessionsTable
{
    public DateTime? SessionTimeEqual { get; set; }
    public Guid? FilmIdEqual { get; set; }
    public FilmAudioOption? FilmAudioOptionEqual { get; set; }
    public FilmFormat? FilmFormatEqual { get; set; }
    public string[]? Includes { get; set; }
    public int PageSize { get; set; } = 10; 
    public int PageNumber { get; set; } = 1; 
}