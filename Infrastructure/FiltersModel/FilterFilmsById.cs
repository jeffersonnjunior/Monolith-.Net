namespace Infrastructure.FiltersModel;

public class FilterFilmsById
{
    public Guid Id { get; set; }
    public string[]? Includes { get; set; }
}