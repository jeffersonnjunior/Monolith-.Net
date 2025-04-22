using Application.Dtos;
using Domain.Entities;

namespace Application.Interfaces.IFactories;

public interface ISeatsFactory
{
    Seats CreateSeat();
    SeatsCreateDto CreateSeatCreateDto();
    SeatsReadDto CreateSeatReadDto();
    SeatsUpdateDto CreateSeatUpdateDto();
    Seats MapToSeat(SeatsCreateDto dto);
    SeatsCreateDto MapToSeatCreateDto(Seats entity);
    SeatsReadDto MapToSeatReadDto(Seats entity);
    SeatsUpdateDto MapToSeatUpdateDto(Seats entity);
    Seats MapToSeatFromUpdateDto(SeatsUpdateDto dto);
}