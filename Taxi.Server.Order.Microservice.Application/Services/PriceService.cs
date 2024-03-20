using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taxi.Server.Order.Microdervice.Domain.Interfaces.Services;
using Taxi.Server.Order.Microdervice.Domain.Models;

namespace Taxi.Server.Order.Microdervice.Application.Services
{
    public class PriceService : IPriceService
    {
        private readonly IOrderService _orderService;

        public PriceService(IOrderService orderService) 
        {
            _orderService = orderService;
        }

        public double GetPrice(string clientUsername, Addres startPoint, Addres endPoint)
        {
            _orderService.CreateOrder(clientUsername, 10d, startPoint, endPoint);
            return 10d;
        }
    }
}
