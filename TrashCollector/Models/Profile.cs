using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrashCollector.Models
{
    public class Profile
    {
        public int ProfileId { get; set; }
        public ICollection<Address> Addresses { get; set; }
        public string ZipCodes { get; set; }
    }
}