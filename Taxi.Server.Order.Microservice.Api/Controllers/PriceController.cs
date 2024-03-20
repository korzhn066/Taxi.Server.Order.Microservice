using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Taxi.Server.Order.Microdervice.Domain.Interfaces.Services;
using Taxi.Server.Order.Microservice.Api.Dto;
using Taxi.Server.Order.Microservice.Api.Helpers;

namespace Taxi.Service.Order.Microservice.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PriceController  : ControllerBase
    {
        private readonly IPriceService _priceService;

        public PriceController(IPriceService priceService)
        {
            _priceService = priceService;
        }

        [HttpPost]
        [Route("GetPrice")]
        public IActionResult GetPrice(GetPriceDto dto)
        {
            var price = _priceService.GetPrice(
                AuthenticateHelper.GetUserName(HttpContext),
                dto.StartPoint, 
                dto.EndPoint);

            return Ok(price);
        }
    }
}
