using Application.Dtos;
using Application.Interfaces.IFactories;
using Domain.Entities;

namespace Application.Factories;

public class SeatsFactory : ISeatsFactory
{
    public Seats CreateSeat()
    {
        return new Seats
        {
            Id = Guid.NewGuid(),
            SeatNumber = 0,
            RowLetter = string.Empty,
            ScreenId = Guid.Empty,
            Screen = null,
            Tickets = new List<Tickets>()
        };
    }

    public SeatsCreateDto CreateSeatCreateDto()
    {
        return new SeatsCreateDto
        {
            SeatNumber = 0,
            RowLetter = string.Empty,
            ScreenId = Guid.Empty
        };
    }

    public SeatsReadDto CreateSeatReadDto()
    {
        return new SeatsReadDto
        {
            Id = Guid.NewGuid(),
            SeatNumber = 0,
            RowLetter = string.Empty,
            ScreenId = Guid.Empty
        };
    }

    public SeatsUpdateDto CreateSeatUpdateDto()
    {
        return new SeatsUpdateDto
        {
            Id = Guid.NewGuid(),
            SeatNumber = 0,
            RowLetter = string.Empty,
            ScreenId = Guid.Empty
        };
    }

    public Seats MapToSeat(SeatsCreateDto dto)
    {
        return new Seats
        {
            Id = Guid.NewGuid(),
            SeatNumber = dto.SeatNumber,
            RowLetter = dto.RowLetter,
            ScreenId = dto.ScreenId,
            Tickets = new List<Tickets>()
        };
    }

    public SeatsCreateDto MapToSeatCreateDto(Seats entity)
    {
        return new SeatsCreateDto
        {
            SeatNumber = entity.SeatNumber,
            RowLetter = entity.RowLetter,
            ScreenId = entity.ScreenId
        };
    }

    public SeatsReadDto MapToSeatReadDto(Seats entity)
    {
        return new SeatsReadDto
        {
            Id = entity.Id,
            SeatNumber = entity.SeatNumber,
            RowLetter = entity.RowLetter,
            ScreenId = entity.ScreenId
        };
    }

    public SeatsUpdateDto MapToSeatUpdateDto(Seats entity)
    {
        return new SeatsUpdateDto
        {
            Id = entity.Id,
            SeatNumber = entity.SeatNumber,
            RowLetter = entity.RowLetter,
            ScreenId = entity.ScreenId
        };
    }

    public Seats MapToSeatFromUpdateDto(SeatsUpdateDto dto)
    {
        return new Seats
        {
            Id = dto.Id,
            SeatNumber = dto.SeatNumber,
            RowLetter = dto.RowLetter,
            ScreenId = dto.ScreenId,
            Tickets = new List<Tickets>()
        };
    }
}