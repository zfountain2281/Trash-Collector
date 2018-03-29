using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace TrashCollector.Models
{
    public class ProfileDetailsViewModel
    {
        public List<Invoice> Invoices { get; set; }
        public double AmountDue { get; set; }
    }

}