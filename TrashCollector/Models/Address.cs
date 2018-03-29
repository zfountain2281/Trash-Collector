using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TrashCollector.Models
{
    public class Address
    {
        //properties
        public int AddressId { get; set; }
        //[Display(Name = "Date of Birth")]
        public string StreetOne { get; set; }

        public City City { get; set; }
        public int CityId { get; set; }

        public State State { get; set; }
        public int StateId { get; set; }

        public ZipCode ZipCode { get; set; }
        public int ZipCodeId { get; set; }

        public float? lat { get; set; }
        public float? lng { get; set; }

        public TrashCollection TrashCollection { get; set; }
        public int? TrashCollectionId { get; set; }

    }
}