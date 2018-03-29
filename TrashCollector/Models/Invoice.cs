using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrashCollector.Models
{
    public class Invoice
    {
        public int InvoiceId { get; set; }
        public string UserId { get; set; }
        public ICollection<Pickup> Pickups { get; set; }
        public double AmountDue { get; set; }
        public bool IsPaid { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DueDate { get; set; }
    }
}