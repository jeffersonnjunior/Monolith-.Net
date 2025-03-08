namespace Infrastructure.Utilities.FiltersModel;

public class FilterTicketsById
{
    public Guid Id { get; set; }
    public string[]? Includes { get; set; }
}