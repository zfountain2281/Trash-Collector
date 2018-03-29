using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrashCollector.Models
{
    public class Pickup
    {
        public int PickupId { get; set; }
        public bool IsInvoiced { get; set; }
        public DateTime DateCompleted { get; set; }
        public double Price { get; set; }
    }
}