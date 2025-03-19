using Application.Dtos;
using Application.Interfaces.IServices;
using Infrastructure.Notifications;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Utilities.FiltersModel;

namespace Api.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
[Produces("application/json")]
public class CustomerDetailsController : Controller
{
    private readonly ICustomerDetailsService _customerDetailsService;
    private readonly NotificationContext _notificationContext;
    public CustomerDetailsController(ICustomerDetailsService customerDetailsService, NotificationContext notificationContext)
    {
        _customerDetailsService = customerDetailsService;
        _notificationContext = notificationContext;
    }
    
    [HttpGet]
    [Route("get-by-id")]
    public IActionResult GetById([FromQuery] FilterCustomerDetailsById filterCustomerDetailsById)
    {
        return Ok(_customerDetailsService.GetById(filterCustomerDetailsById));
    }
    
    [HttpGet]
    [Route("get-filter")]
    public IActionResult GetFilter([FromQuery] FilterCustomerDetailsTable filter)
    {
        return Ok(_customerDetailsService.GetFilter(filter));
    }
    
    [HttpPost]
    [Route("add")]
    public IActionResult Add([FromBody] CustomerDetailsCreateDto customerDetailsCreateDto)
    {
        var customerDetails = _customerDetailsService.Add(customerDetailsCreateDto);
        
        if (_notificationContext.HasNotifications()) return BadRequest(new { errors = _notificationContext.GetNotifications() });
        
        var uri = Url.Action(nameof(GetById), new { id = customerDetails.Id });
        return Created(uri, customerDetails);
    }
    
    [HttpPut]
    [Route("update")]
    public IActionResult Update([FromBody] CustomerDetailsUpdateDto customerDetailsUpdateDto)
    {
        _customerDetailsService.Update(customerDetailsUpdateDto);
        return Ok();
    }
    
    [HttpDelete]
    [Route("{id}")]
    public IActionResult Delete(Guid id)
    {
        _customerDetailsService.Delete(id);
        return Ok();
    }
}
