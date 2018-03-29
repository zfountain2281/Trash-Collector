using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using TrashCollector.Models;

[assembly: OwinStartupAttribute(typeof(TrashCollector.Startup))]
namespace TrashCollector
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesandUsers();
        }


        // In this method we will create default User roles and Admin user for login   
        private void createRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


            // In Startup iam creating first Admin Role and creating a default Admin User    
            if (!roleManager.RoleExists("Admin"))
            {
                // first we create Admin role   
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                //Here we create a Admin super user who will maintain the website
                //TO DO: move this to the seed method                  

                var user = new ApplicationUser();
                user.UserName = "admin";
                user.Email = "ztrain71@att.net";
                user.Profile = new Profile();
                string userPWD = "Hockey.271";
                var chkUser = UserManager.Create(user, userPWD);
                //Add default User to Role Admin   
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Admin");
                }
            }

            // creating Creating Employee role    
            if (!roleManager.RoleExists("Employee"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Employee";
                roleManager.Create(role);

                //Declare user for re-use
                ApplicationUser user;

                //Add zfountain2281
                user = new ApplicationUser();
                user.UserName = "zfountain2281";
                user.Email = "ztrain88@att.net";
                user.Profile = new Profile();
                user.Profile.ZipCodes = "53090";
                string userPWD = "Hockey.271";
                var chkUser = UserManager.Create(user, userPWD);
                //Add default User to Role Employee   
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Employee");
                }
            }

            // creating Creating Customer role    
            if (!roleManager.RoleExists("Customer"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Customer";
                roleManager.Create(role);

                //Declare user for re-use
                ApplicationUser user;

                //Add Customer
                user = new ApplicationUser();
                user.UserName = "Customer";
                user.Email = "customer@example.com";
                user.Profile = new Profile();
                user.Profile.Addresses = new List<Address>();
                string userPWD = "Hockey.271";
                var chkUser = UserManager.Create(user, userPWD);
                //Add default User to Role Employee   
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Customer");
                }

                //Add 53090 ZipCode
                ZipCode zipCode = new ZipCode();
                zipCode.Number = "53090";
                context.ZipCodes.Add(zipCode);
                context.SaveChanges();

                //Add West Bend
                City city = new City();
                city.Name = "West Bend";
                context.Cities.Add(city);
                context.SaveChanges();

                //Declare address, trash collection objects for re-use
                Address address;
                TrashCollection trashCollection;

                //Add House Addresses1
                address = new Address();
                address.StreetOne = "713 Oakfield St";
                address.CityId = city.CityId;
                address.StateId = 50; //Wisconsin state id
                address.ZipCodeId = zipCode.ZipCodeId;
                address.lat = 43.042921f;
                address.lng = -87.909817f;
                trashCollection = new TrashCollection();
                trashCollection.PickUpDay = "Wednesday";
                trashCollection.StartDate = Convert.ToDateTime("03/29/2018");
                address.TrashCollection = trashCollection;
                context.Addresses.Add(address);
                context.SaveChanges();
                //Add the address to StarbuckGuy's List
                user.Profile.Addresses.Add(address);
                context.SaveChanges();

                //Add Starbucks Addresses2
                address = new Address();
                address.StreetOne = "323 E Wisconsin Ave";
                address.CityId = city.CityId;
                address.StateId = 50; //Wisconsin state id
                address.ZipCodeId = zipCode.ZipCodeId;
                address.lat = 43.038226f;
                address.lng = -87.906829f;
                trashCollection = new TrashCollection();
                trashCollection.PickUpDay = "Wednesday";
                trashCollection.StartDate = Convert.ToDateTime("03/29/2018");
                address.TrashCollection = trashCollection;
                context.Addresses.Add(address);
                context.SaveChanges();
                //Add the address to StarbuckGuy's List
                user.Profile.Addresses.Add(address);
                context.SaveChanges();

                //Add Starbucks Addresses3
                address = new Address();
                address.StreetOne = "544 E Ogden Ave #500";
                address.CityId = city.CityId;
                address.StateId = 50; //Wisconsin state id
                address.ZipCodeId = zipCode.ZipCodeId;
                address.lat = 43.048397f;
                address.lng = -87.905368f;
                trashCollection = new TrashCollection();
                trashCollection.PickUpDay = "Wednesday";
                trashCollection.StartDate = Convert.ToDateTime("03/29/2018");
                address.TrashCollection = trashCollection;
                context.Addresses.Add(address);
                context.SaveChanges();
                //Add the address to StarbuckGuy's List
                user.Profile.Addresses.Add(address);
                context.SaveChanges();

                //Add Starbucks Addresses4
                address = new Address();
                address.StreetOne = "1677 N Farwell Ave";
                address.CityId = city.CityId;
                address.StateId = 50; //Wisconsin state id
                address.ZipCodeId = zipCode.ZipCodeId;
                address.lat = 43.052802f;
                address.lng = -87.892719f;
                trashCollection = new TrashCollection();
                trashCollection.PickUpDay = "Wednesday";
                trashCollection.StartDate = Convert.ToDateTime("03/29/2018");
                address.TrashCollection = trashCollection;
                context.Addresses.Add(address);
                context.SaveChanges();
                //Add the address to StarbuckGuy's List
                user.Profile.Addresses.Add(address);
                context.SaveChanges();

                //Add Starbucks Addresses5
                address = new Address();
                address.StreetOne = "800 E Wisconsin Ave";
                address.CityId = city.CityId;
                address.StateId = 50; //Wisconsin state id
                address.ZipCodeId = zipCode.ZipCodeId;
                address.lat = 43.040005f;
                address.lng = -87.900157f;
                trashCollection = new TrashCollection();
                trashCollection.PickUpDay = "Wednesday";
                trashCollection.StartDate = Convert.ToDateTime("03/29/2018");
                address.TrashCollection = trashCollection;
                context.Addresses.Add(address);
                context.SaveChanges();
                //Add the address to StarbuckGuy's List
                user.Profile.Addresses.Add(address);
                context.SaveChanges();

                //Add Starbucks Addresses6
                address = new Address();
                address.StreetOne = "326 N Water St";
                address.CityId = city.CityId;
                address.StateId = 50; //Wisconsin state id
                address.ZipCodeId = zipCode.ZipCodeId;
                address.lat = 43.034354f;
                address.lng = -87.908515f;
                trashCollection = new TrashCollection();
                trashCollection.PickUpDay = "Tuesday";
                trashCollection.StartDate = Convert.ToDateTime("03/29/2018");
                address.TrashCollection = trashCollection;
                context.Addresses.Add(address);
                context.SaveChanges();
                //Add the address to StarbuckGuy's List
                user.Profile.Addresses.Add(address);
                context.SaveChanges();

            }

        }
    }
}
