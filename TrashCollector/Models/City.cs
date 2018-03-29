using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TrashCollector.Models
{
    public class City
    {
        public int CityId { get; set; }

        [Display(Name = "City_Name")]
        public string Name { get; set; }
    }
}