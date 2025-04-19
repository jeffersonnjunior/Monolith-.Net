using Application.Dtos;
using Domain.Entities;

namespace Application.Interfaces.IFactory;

public interface IFilmeFactory
{
    Films CreateFilme();
    FilmsCreateDto CreateFilmeCreateDto();
    FilmsReadDto CreateFilmeReadDto();
    FilmsUpdateDto CreateFilmeUpdateDto();
    Films MapToFilme(FilmsCreateDto dto);
    FilmsCreateDto MapToFilmeCreateDto(Films entity);
    FilmsReadDto MapToFilmeReadDto(Films entity);
    FilmsUpdateDto MapToFilmeUpdateDto(Films entity);
    Films MapToFilmeFromUpdateDto(FilmsUpdateDto dto);
}