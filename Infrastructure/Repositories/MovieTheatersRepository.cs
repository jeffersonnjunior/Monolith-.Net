using Domain.Entities;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class MovieTheatersRepository : BaseRepository<MovieTheaters>, IMovieTheatersRepository
{
    public MovieTheatersRepository(DbContext context, IUnitOfWork unitOfWork) : base(context, unitOfWork)
    {
    }
}
