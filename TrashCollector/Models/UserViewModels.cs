using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace TrashCollector.Models
{
    public class IndexUserViewModel
    {
        public ApplicationUser[] Users { get; set; }
        public Role[] UserRoles { get; set; }
    }

    public class EditUserViewModel
    {
        public ApplicationUser User { get; set; }
        public string UserRole { get; set; }
    }

}