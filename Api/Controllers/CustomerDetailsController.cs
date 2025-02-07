using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class CustomerDetailsController : Controller
{
    private readonly ICustomerDetailsService _customerDetailsService;
    public CustomerDetailsController(ICustomerDetailsService customerDetailsService)
    {
        _customerDetailsService = customerDetailsService;
    }
}
