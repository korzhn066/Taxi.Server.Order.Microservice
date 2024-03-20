using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taxi.Server.Order.Microdervice.Domain.Models
{
    public class Addres
    {
        public string Name { get; set; } = null!;
        public Geolocation Geolocation { get; set; } = null!;
    }
}
