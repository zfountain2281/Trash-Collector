using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TrashCollector.Models
{
    public class ZipCode
    {
        public int ZipCodeId { get; set; }

        [Required]
        [Display(Name = "ZipCode_Number")]
        public string Number { get; set; }
    }
}