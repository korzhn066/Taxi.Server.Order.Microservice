using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taxi.Server.Order.Microdervice.Domain.Models;

namespace Taxi.Server.Order.Microdervice.Domain.Interfaces.Services
{
    public interface ITrackerService
    {
        void EndWork(string username);
        void ChangeGeolocation(string username, Geolocation geolocation);
        string? GetNearestDriverExludingList(List<string> driverUsernames, Geolocation geolocation);
    }
}
