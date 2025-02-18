namespace Infrastructure.Utilities.FiltersModel;

public class ReturnTable<T> where T : class
{
    public ReturnTable()
    {
        ItensList = new List<T>();
    }

    public ReturnTable(int totalRegister, int totalRegisterFilter, int totalPages)
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