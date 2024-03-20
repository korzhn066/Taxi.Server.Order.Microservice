using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.SignalR;
using Taxi.Server.Order.Microdervice.Domain.Interfaces.Services;
using Taxi.Server.Order.Microdervice.Domain.Models;
using Taxi.Server.Order.Microservice.Api.Helpers;
using Taxi.Server.Order.Microservice.Api.Hubs;

namespace Taxi.Server.Order.Microservice.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IHubContext<OrderHub> _hubContext;
        private readonly IOrderService _orderService;
        
        public OrderController(
            IHubContext<OrderHub> hubContext,
            IOrderService orderService)
        {
            _hubContext = hubContext;
            _orderService = orderService;
        }

        private async Task SendToDriver(string username, OrderInformation orderInformation)
        {
            await _hubContext.Clients.User(username).SendAsync("order", orderInformation);
        }

        [HttpPost]
        [Route("FindDriver")]
        public async Task<IActionResult> FindDriver()
        {
            CancellationTokenSource cancellationTokenFindSource = new();
            CancellationToken cancellationTokenFind = cancellationTokenFindSource.Token;
            var driver = await _orderService.FindDriverAsync(
                AuthenticateHelper.GetUserName(HttpContext), 
                SendToDriver, 
                cancellationTokenFindSource, 
                cancellationTokenFind);
            

            return Ok(driver);
        }

        [HttpPost]
        [Route("ApplyOrder")]
        [Authorize(Roles = "Driver")]
        public IActionResult ApplyOrder(string clientUsername)
        {
            _orderService.ApplyOrder(clientUsername, AuthenticateHelper.GetUserName(HttpContext));

            return NoContent();
        }

        [HttpPost]
        [Route("RefuseOrder")]
        [Authorize(Roles = "Driver")]
        public IActionResult RefuseOrder(string clientUsername)
        {
            _orderService.RefuseOrder(clientUsername);

            return Ok();
        }

        [HttpPost]
        [Route("CancelOrder")]
        public IActionResult CancelOrder()
        {
            _orderService.CancelOrder(AuthenticateHelper.GetUserName(HttpContext));

            return Ok();
        }
    }
}
