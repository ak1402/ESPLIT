using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DataAccessLayer.Models;
using SplitWise.Custom;

namespace SplitWise.Controllers
{
    public class AccountController : Controller
    {
        SplitwiseEntities db = new SplitwiseEntities();
        //
        // GET: /Account/
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string name, string password, string retUrl)
        {
            if (db.SplitwiseUsers.FirstOrDefault(usr => usr.UserName == name && usr.UserPassword == password) != null)
            {
                SplitwiseUser SUser = db.SplitwiseUsers.FirstOrDefault(usr => usr.UserName == name && usr.UserPassword == password);
                FormsAuthentication.SetAuthCookie(name, false);
                Session["UserID"] = db.SplitwiseUsers.FirstOrDefault(usr => usr.UserName == name && usr.UserPassword == password).UserID;
                bool flag = false;
                if (SUser.SplitWiseUserRoles.RoleName == "Admin")
                {
                    flag = true;
                    Session["IsAdmin"] = true;
                }
                else
                {
                    flag = false;
                    Session["IsAdmin"] = false;
                }
                return RedirectToAction("Welcome", "UserDetails");
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }
        [CustomAdminFilter]
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(SplitwiseUser User)
        {
            if (ModelState.IsValid)
            {
                db.SplitwiseUsers.Add(User);
                db.SaveChanges();
            }
            return RedirectToAction("Welcome", "UserDetails");
        }

        [HttpGet]
        public ActionResult Manage()
        {
            int currentUser = Convert.ToInt32(Session["UserId"]);
            //SplitwiseUser splitwiseUser = db.SplitwiseUsers.FirstOrDefault(user => user.UserID == currentUser);
            //int? roleId = splitwiseUser.UserRole;
            //TempData["UserRoleName"] = db.SplitWiseUserRoles.FirstOrDefault(user => user.RoleId == roleId).RoleName;
            //ViewBag.UserRoleName = db.SplitWiseUserRoles.FirstOrDefault(user => user.RoleId == roleId).RoleName.ToString();
            
            return View(getUserObject(currentUser));
        }
        public SplitwiseUser getUserObject(int currentUser)
        {
            
            SplitwiseUser splitwiseUser = db.SplitwiseUsers.FirstOrDefault(user => user.UserID == currentUser);
            int? roleId = splitwiseUser.UserRole;
            TempData["UserRoleName"] = db.SplitWiseUserRoles.FirstOrDefault(user => user.RoleId == roleId).RoleName;
            return splitwiseUser;
        }
        [HttpPost]
        [ActionName("Manage")]
        public ActionResult Manage_Post(SplitwiseUser splitWiseUser)
        {
            int currentUser = Convert.ToInt32(Session["UserId"]);
            int? UserRole = db.SplitwiseUsers.FirstOrDefault(user => user.UserID == currentUser).UserRole;
            int? roleId = splitWiseUser.UserRole;
            TempData["UserRoleName"] = db.SplitWiseUserRoles.FirstOrDefault(user => user.RoleId == roleId).RoleName;
                     
            if (ModelState.IsValid)
            {
                db.updt_User(currentUser, splitWiseUser.UserName, splitWiseUser.UserPassword, UserRole);
                db.SaveChanges();
                return RedirectToAction("Welcome", "UserDetails");
            }
            return View(getUserObject(currentUser));
        }

        [HttpGet]
        [CustomAdminFilter]
        public ActionResult ManageOthersAccount()
        {
            int currentAdmin = Convert.ToInt32(Session["UserId"]);
            List<SplitwiseUser> splitWiseUser = db.SplitwiseUsers.Where(user => user.UserID != currentAdmin).ToList();
            return View(splitWiseUser);
        }
        [HttpGet]
        [CustomAdminFilter]
        public ActionResult EditOthers(int id)
        {
            SplitwiseUser splitWiseUser = db.SplitwiseUsers.FirstOrDefault(userId => userId.UserID == id);
            List<SplitWiseUserRoles> splitWiseUserRoles = db.SplitWiseUserRoles.ToList();
            List<SelectListItem> ListSelectItems = new List<SelectListItem>();
            foreach(SplitWiseUserRoles splitWiseUserRole in splitWiseUserRoles)
            {
                ListSelectItems.Add(new SelectListItem() { Text = splitWiseUserRole.RoleName, Value = splitWiseUserRole.RoleId.ToString() });
            }
            ViewBag.UserRole = ListSelectItems;
            return View(splitWiseUser);
        }
        [HttpPost]
        [CustomAdminFilter]
        public ActionResult EditOthers(SplitwiseUser User)
        {
            if (ModelState.IsValid)
            {
                db.Entry(User).State = System.Data.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ManageOthersAccount");
            }
            List<SplitWiseUserRoles> splitWiseUserRoles = db.SplitWiseUserRoles.ToList();
            List<SelectListItem> ListSelectItems = new List<SelectListItem>();
            foreach (SplitWiseUserRoles splitWiseUserRole in splitWiseUserRoles)
            {
                ListSelectItems.Add(new SelectListItem() { Text = splitWiseUserRole.RoleName, Value = splitWiseUserRole.RoleId.ToString() });
            }
            ViewBag.UserRole = ListSelectItems;
            return View(User);
            
        }
        public ActionResult DetailsOthers(int id)
        {
            SplitwiseUser userDetails = db.SplitwiseUsers.FirstOrDefault(user => user.UserID == id);
            return View(userDetails);
        }
        [HttpGet]
        public ActionResult DeleteOthers(int id)
        {
            SplitwiseUser user = db.SplitwiseUsers.FirstOrDefault(usr => usr.UserID == id);
            return View(user);
        }
        [HttpPost]
        [ActionName("DeleteOthers")]
        public ActionResult DeleteOthers_Post(int id)
        {
            tbl_Balance Balancetbl = db.tbl_Balance.FirstOrDefault(bal => bal.UserId == id);
            SplitwiseUser spUser = db.SplitwiseUsers.FirstOrDefault(usr => usr.UserID == id);
            if (Balancetbl.Balance != 0)
            {
                return Content("User dint pay his debt");
            }
            else
            {
                db.tbl_Balance.Remove(Balancetbl);
                db.SplitwiseUsers.Remove(spUser);
                db.SaveChanges();
                return RedirectToAction("ManageOthersAccount");
            }
        }
    }
}
