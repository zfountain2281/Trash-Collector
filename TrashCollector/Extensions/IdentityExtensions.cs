using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using TrashCollector.Models;

namespace TrashCollector.Extensions
{

    public static class IdentityExtensions
    {
        public static string GetProfileId(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("ProfileId");
            // Test for null to avoid issues during local testing
            return (claim != null) ? claim.Value : string.Empty;
        }

        public static void SetProfileID()
        {

        }

        public static bool HasProfile(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("ProfileId");
            // Test for null to avoid issues during local testing
            return (claim.Value != "") ? true : false;
        }

        public static string GetUserRole()
        {
            return "Customer";
        }

    }
}