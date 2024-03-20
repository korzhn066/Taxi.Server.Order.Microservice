using Taxi.Server.Order.Microdervice.Domain.Interfaces.Services;
using Taxi.Server.Order.Microdervice.Domain.Models;

namespace Taxi.Server.Order.Microdervice.Application.Services
{
    public class TrackerService : ITrackerService
    {
        private readonly Dictionary<string, Geolocation> _drivers;
        public TrackerService()
        {
            _drivers = new Dictionary<string, Geolocation>();
        }
        public void ChangeGeolocation(string username, Geolocation geolocation)
        {
            _drivers[username] = geolocation;
        }

        public string? GetNearestDriverExludingList(List<string> driverUsernames, Geolocation geolocation)
        {
            string? result = null;
            var distance = 0d;

            foreach (var driver in _drivers)
            {
                if (result is null)
                {
                    result = driver.Key;
                    distance = GetDistance(geolocation, driver.Value);
                }
                else
                {
                    var newDistance = GetDistance(geolocation, driver.Value);

                    if (newDistance < distance && !driverUsernames.Contains(driver.Key))
                    {
                        result = driver.Key;
                        distance = newDistance;
                    }
                }
            }

            return result;
        }

        private static double GetDistance(Geolocation start, Geolocation end)
        {
            return Math.Sqrt((start.Latitude - end.Latitude) * (start.Latitude - end.Latitude) +
                (start.Longitude - end.Longitude) * (start.Longitude - end.Longitude));
        }

        public void EndWork(string username)
        {
            _drivers.Remove(username);
        }
    }
}
