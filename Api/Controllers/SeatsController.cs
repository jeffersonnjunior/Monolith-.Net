using Application.Dtos;
using Application.Interfaces.IServices;
using Infrastructure.Utilities.FiltersModel;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Produces("application/json")]
[Route("api/Seats")]
public class SeatsController : Controller
{
    private readonly ISeatsService _seatsService;
    public SeatsController(ISeatsService seatsService)
    {
        _seatsService = seatsService;
    }
    
    [HttpGet]
    [Route("get-by-id")]
    public IActionResult GetById([FromQuery] FilterSeatsById filterSeatsById)
    {
        var seatsReadDto = _seatsService.GetById(filterSeatsById);
        
        return Ok(seatsReadDto);
    }
    
    [HttpGet]
    [Route("get-filter")]
    public IActionResult GetFilter([FromQuery] FilterSeatsTable filter)
    {
        var filterReturn = _seatsService.GetFilter(filter);
        
        return Ok(filterReturn);
    }
    
    [HttpPost]
    [Route("add")]
    public IActionResult Add([FromBody] SeatsCreateDto seatsCreateDto)
    {
        var seatsUpdateDto = _seatsService.Add(seatsCreateDto);
        
        return Ok(seatsUpdateDto);
    }
    
    [HttpPut]
    [Route("update")]
    public IActionResult Update([FromBody] SeatsUpdateDto seatsUpdateDto)
    {
        _seatsService.Update(seatsUpdateDto);
        
        return Ok();
    }
    
    [HttpDelete]
    [Route("{id}")]
    public IActionResult Delete(Guid id)
    {
        _seatsService.Delete(id);
        return Ok();
    }
}