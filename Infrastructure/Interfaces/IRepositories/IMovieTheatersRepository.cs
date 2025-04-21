using Domain.Entities;
using Infrastructure.FiltersModel;

namespace Infrastructure.Interfaces.IRepositories;

public interface IMovieTheatersRepository : IBaseRepository<MovieTheaters>
{
    MovieTheaters GetByElement(FilterByItem filterByItem);
    FilterReturn<MovieTheaters> GetFilter(FilterMovieTheatersTable filter);
    bool ValidateInput(object dto, bool isUpdate, MovieTheaters existingMovieTheater = null);
}
