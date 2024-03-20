using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Taxi.Server.Order.Microdervice.Domain.Interfaces.Services;
using Taxi.Server.Order.Microdervice.Domain.Models;

namespace Taxi.Server.Order.Microdervice.Application.Services
{
    public class Order 
    {
        public string? ClientUsername { get; set; }
        public string? DriverUsername { get; set; }
        public Addres StartPoint { get; set; } = null!;
        public Addres EndPoint { get; set; } = null!;
        public List<string> RefusedDrivers { get; set; } = new();
        public double Price { get; set; }
        public CancellationTokenSource CancellationTokenOfferSource { get; set; } = null!;
        public CancellationTokenSource CancellationTokenFindSource { get; set; } = null!;
    }

    public class OrderService : IOrderService
    {
        private readonly ITrackerService _trackerService;
        private readonly Dictionary<string, Order> _orders = new();
        
        public OrderService(ITrackerService trackerService)
        {
            _trackerService = trackerService;
        }

        public async Task<string?> FindDriverAsync(
            string clientUsername, 
            Func<string, OrderInformation, Task> sendToDriver,
            CancellationTokenSource cancellationTokenFindSource,
            CancellationToken cancellationTokenFind = default)
        {
            if (!_orders.ContainsKey(clientUsername))
                throw new ArgumentNullException(nameof(_orders));

            CancellationTokenSource cancellationTokenOfferSource = new();
            CancellationToken cancellationTokenOffer = cancellationTokenOfferSource.Token;

            _orders[clientUsername].CancellationTokenFindSource = cancellationTokenFindSource;
            _orders[clientUsername].CancellationTokenOfferSource = cancellationTokenOfferSource;

            while (_orders[clientUsername].DriverUsername is null)
            {
                if (cancellationTokenFind.IsCancellationRequested) 
                    break;

                await OfferOrder(clientUsername, sendToDriver, cancellationTokenOffer);
            }

            return _orders[clientUsername].DriverUsername;
        }

        private async Task OfferOrder(
            string clientUsername,
            Func<string, OrderInformation, Task> sendToDriver,
            CancellationToken cancellationToken)
        {
            var driver = _trackerService.GetNearestDriverExludingList(
                _orders[clientUsername].RefusedDrivers, 
                new Geolocation());

            _orders[clientUsername].RefusedDrivers.Add(driver);

            await sendToDriver(
                driver, 
                new OrderInformation
                {
                    Price = _orders[clientUsername].Price,
                    StartPoint = _orders[clientUsername].StartPoint,
                    EndPoint = _orders[clientUsername].EndPoint,
                });

            await Task.Delay(10000, cancellationToken); // - это ожидание
        }

        public void ApplyOrder(string clientUsername, string driverUsername)
        {
            _orders[clientUsername].ClientUsername = clientUsername;
            _orders[driverUsername].DriverUsername = driverUsername;
            _orders[clientUsername].CancellationTokenOfferSource.Cancel();
        }

        public void RefuseOrder(string clientUsername)
        {
            _orders[clientUsername].CancellationTokenOfferSource.Cancel();
        }

        public void CancelOrder(string clientUsername)
        {
            _orders[clientUsername].CancellationTokenFindSource.Cancel();
        }

        public void CreateOrder(
            string clientUsername, 
            double price,
            Addres startPoint,
            Addres endPoint)
        {
            _orders[clientUsername] = new Order
            {
                Price = price,
                StartPoint = startPoint,
                EndPoint = endPoint
            };
        }
    }
}
