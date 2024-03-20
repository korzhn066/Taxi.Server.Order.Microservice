using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taxi.Server.Order.Microdervice.Domain.Models;

namespace Taxi.Server.Order.Microdervice.Domain.Interfaces.Services
{
    public interface IOrderService
    {
        Task<string?> FindDriverAsync(
            string clientUsername,
            Func<string, OrderInformation, Task> sendToDriver,
            CancellationTokenSource cancellationTokenFindSource,
            CancellationToken cancellationTokenFind = default);
        void ApplyOrder(string clientUsername, string driverUsername);
        void RefuseOrder(string clientUsername);
        void CancelOrder(string clientUsername);
        void CreateOrder(
            string clientUsername, 
            double price,
            Addres startPoint,
            Addres endPoint);
    }
}
