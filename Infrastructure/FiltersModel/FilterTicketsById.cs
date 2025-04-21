namespace Infrastructure.FiltersModel;

public class FilterTicketsById
{
    public Guid Id { get; set; }
    public string[]? Includes { get; set; }
}