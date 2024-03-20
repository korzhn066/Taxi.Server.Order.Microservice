using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taxi.Server.Order.Microdervice.Domain.Models
{
    public class OrderInformation
    {
        public Addres StartPoint { get; set; } = null!;
        public Addres EndPoint { get; set; } = null!;
        public double Distance { get; set; }
        public double Price { get;set; }
        public string ClientUsername { get; set; } = null!;
    }
}
