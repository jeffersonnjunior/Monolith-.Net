using Application.Dtos;
using Domain.Entities;

namespace Application.Interfaces.IFactories;

public interface IMovieTheatersFactory
{
    MovieTheaters CreateMovieTheater();
    MovieTheatersCreateDto CreateMovieTheaterCreateDto();
    MovieTheatersReadDto CreateMovieTheaterReadDto();
    MovieTheatersUpdateDto CreateMovieTheaterUpdateDto();
    MovieTheaters MapToMovieTheater(MovieTheatersCreateDto dto);
    MovieTheatersCreateDto MapToMovieTheaterCreateDto(MovieTheaters entity);
    MovieTheatersReadDto MapToMovieTheaterReadDto(MovieTheaters entity);
    MovieTheatersUpdateDto MapToMovieTheaterUpdateDto(MovieTheaters entity);
    MovieTheaters MapToMovieTheaterFromUpdateDto(MovieTheatersUpdateDto dto);
}