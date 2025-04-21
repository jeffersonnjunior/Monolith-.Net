namespace Infrastructure.Utilities.FiltersModel;

public class FilterCustomerDetailsTable
{
    public string? NameContains { get; set; }
    public string? EmailContains { get; set; }
    public int? AgeEqual { get; set; }
    public string[]? Includes { get; set; }
    public int PageSize { get; set; } = 10; 
    public int PageNumber { get; set; } = 1; 
}