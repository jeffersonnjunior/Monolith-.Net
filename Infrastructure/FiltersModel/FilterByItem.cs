namespace Infrastructure.FiltersModel;

public class FilterByItem
{
    public required string Field { get; set; } = "Id";
    public required object Value { get; set; }
    public required string Key { get; set; } = "Equal";
    public string[]? Includes { get; set; }
}
