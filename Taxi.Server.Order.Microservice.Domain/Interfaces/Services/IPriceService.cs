using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taxi.Server.Order.Microdervice.Domain.Models;

namespace Taxi.Server.Order.Microdervice.Domain.Interfaces.Services
{
    public interface IPriceService
    {
        double GetPrice(string clientUsername, Addres startPoint, Addres endPoint);
    }
}
