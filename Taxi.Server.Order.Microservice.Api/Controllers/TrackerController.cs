using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Taxi.Server.Order.Microdervice.Domain.Interfaces.Services;
using Taxi.Server.Order.Microdervice.Domain.Models;
using Taxi.Server.Order.Microservice.Api.Helpers;

namespace Taxi.Service.Order.Microservice.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Driver")]
    public class TrackerController : ControllerBase
    {
        private readonly ITrackerService _trackerService;

        public TrackerController(ITrackerService trackerService)
        {
            _trackerService = trackerService;
        }

        [HttpPost]
        [Route("ChangeGeolocation")]
        public IActionResult ChangeGeolocation(Geolocation geolocation)
        {
            _trackerService.ChangeGeolocation(
                AuthenticateHelper.GetUserName(HttpContext), 
                geolocation);

            return NoContent();
        }

        [HttpPost]
        [Route("EndWork")]
        public IActionResult EndWork()
        {
            _trackerService.EndWork(AuthenticateHelper.GetUserName(HttpContext));

            return NoContent();
        }
    }
}
