using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TrashCollector.Models;
using System.Web.Security;
using TrashCollector.Extensions;

namespace TrashCollector.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: User
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            if (!User.IsInRole("Admin"))
            {
                return HttpNotFound();
            }
            //get all the users
            var users = db.Users.ToList();
            //get all the user roles
            var roles = GetRolesFromUsers(users);

            //create the view model
            var viewModel = new IndexUserViewModel()
            {
                Users = users.ToArray(),
                UserRoles = roles.ToArray()
            };

            return View(viewModel);
        }

        IEnumerable<Role> GetRolesFromUsers(IEnumerable<ApplicationUser> users)
        {
            List<Role> userRoles = new List<Role>();
            foreach(var user in users)
            {
                var userRole = GetUserRole(user.Id);
                Role role = GetRole(userRole);
                userRoles.Add(role);
            }
            return userRoles;
        }

        // GET: User/Edit/Id
        public ActionResult Edit(string userId)
        {
            if ( userId == null || userId == "" )
            {
                return HttpNotFound();
            }

            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            if (!User.IsInRole("Admin"))
            {
                return HttpNotFound();
            }
            var user = db.Users
                .Include(u => u.Profile)
                .First(u => u.Id == userId);
            var userRole = GetUserRole(user.Id);
            var role = GetRole(userRole);
            var viewModel = new EditUserViewModel
            {
                User = user,
                UserRole = role.Name              
            };
            //var res = db.Database.SqlQuery("Select * from dbo.AspNetUserRoles").ToList<UserRole>();
            //var userRoles = db.Database.SqlQuery<string>("SELECT * FROM dbo.AspNetUserRoles").ToList();
            //get the user's role
            return View(viewModel);
        }

        UserRole GetUserRole(string userId)
        {
            return db.Database.SqlQuery<UserRole>("Select * from dbo.AspNetUserRoles").Where(x => x.UserId == userId).First();
        }

        Role GetRole(UserRole userRole)
        {
            return db.Database.SqlQuery<Role>("Select * from dbo.AspNetRoles").Where(x => x.Id == userRole.RoleId).First();
        }

        string GetRoleId(string roleName)
        {
            var role = db.Database.SqlQuery<Role>("Select * from dbo.AspNetRoles").Where(x => x.Name == roleName).First();
            return role.Id;
        }

        // POST: Address/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Email,UserName")] ApplicationUser user)
        public ActionResult Edit(string userId, string userName, string email, string userRole, string zipCodes)
        {
            zipCodes = zipCodes.Replace(" ", String.Empty);
            
            //save user info
            var userToUpdate = db.Users
                .Include(u => u.Profile)
                .First(u => u.Id == userId);
            if (ModelState.IsValid)
            {
                userToUpdate.UserName = userName;
                userToUpdate.Email = email;
                db.SaveChanges();
            }
            
            //save role info
            UserRole role = GetUserRole(userId);
            string roleId = GetRoleId(userRole);
            role.RoleId = roleId;
            string command = "UPDATE dbo.AspNetUserRoles SET RoleId = '" + roleId + "' WHERE UserId = '" + role.UserId + "'";
            db.Database.ExecuteSqlCommand(command);

            //save profile info
            int profileId = Convert.ToInt32(userToUpdate.Profile.ProfileId);
            var profile = db.Profiles.First(p => p.ProfileId == profileId);
            if (zipCodes != "")
            {
                profile.ZipCodes = zipCodes;
            }
            db.SaveChanges();



            //ViewBag.CityId = new SelectList(db.Cities, "CityId", "Name", address.CityId);
            //ViewBag.StateId = new SelectList(db.States, "StateId", "Name", address.StateId);
            //ViewBag.ZipCodeId = new SelectList(db.ZipCodes, "ZipCodeId", "Number", address.ZipCodeId);
            return RedirectToAction("Index", "User");
        }

        //Helper Methods
        public Boolean isAdminUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());
                if (s[0].ToString() == "Admin")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
    }
}