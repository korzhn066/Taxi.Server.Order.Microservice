
using Taxi.Server.Order.Microdervice.Domain.Models;

namespace Taxi.Server.Order.Microservice.Api.Dto
{
    public class GetPriceDto
    {
        public Addres StartPoint { get; set; } = null!;
        public Addres EndPoint { get; set;} = null!;
    }
}
