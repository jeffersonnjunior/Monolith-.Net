namespace Infrastructure.Utilities.FiltersModel;

public class FilterReturn<T> where T : class
{
    public FilterReturn()
    {
        ItensList = new List<T>();
    }

    public FilterReturn(int totalRegister, int totalRegisterFilter, int totalPages)
    {
        TotalRegister = totalRegister;
        TotalRegisterFilter = totalRegisterFilter;
        TotalPages = totalPages;
        ItensList = new List<T>();
    }

    public int TotalRegister { get; set; }

    public int TotalRegisterFilter { get; set; }

    public int TotalPages { get; set; }

    public IEnumerable<T> ItensList { get; set; }
}