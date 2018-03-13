using PRSProject.Models;
using PRSProject.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace PRSProject.Controllers
{
    public class UsersController : Controller
    {
        PRSDbContext db = new PRSDbContext();

        // return Json objects including the JsonRequestBehavior.AllowGet
        private ActionResult Js(object data)
        {
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        // Try Catch exception message for SaveChanges()
        private ActionResult TrySave(string actionResult)
        {
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json(new JsonMessage("Failure", ex.Message), JsonRequestBehavior.AllowGet);
            }
            return Json(new JsonMessage("Success", "User was " + actionResult));
        }

        //// /Users/SearchByName? name=xyz
        //public ActionResult SearchByName(string name)
        //{
        //    if (name == null)
        //    {
        //        return Js(new JsonMessage("Failure", "Could not find any customers with that name."));
        //    }
        //    List<User> users = db.Users.Where(c => c.Name.Contains(name)).ToList();
        //    return Js(customers);
        //}

        public ActionResult List()
        {
            return Json(db.Users.ToList(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Get(int? id)
        {
            if (id == null)
            {
                return Json(new JsonMessage("Failure", "Id is null"), JsonRequestBehavior.AllowGet);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return Json(new JsonMessage("Failure", "User does not exist. Do you have the correct Id?"), JsonRequestBehavior.AllowGet);
            }
            return Json(user, JsonRequestBehavior.AllowGet);
        }

        // [POST] /Customers/Create
        public ActionResult Create([FromBody] User user)
        {
            user.Active = true;
            user.DateCreated = DateTime.Now;
            if (!ModelState.IsValid)
            {
                return Js(new JsonMessage("Failure", "ModelState is not valid"));
            }
            
            db.Users.Add(user);
            return TrySave("created.");

        }

        // [POST] /Customers/Change
        public ActionResult Change([FromBody] User user)
        {            
            User tempUser = db.Users.Find(user.Id);
            if (tempUser == null)
            {
                return Js(new JsonMessage("Failure", "Record of user to be changed does not exist"));
            }
            tempUser.UserName = user.UserName;
            tempUser.Password = user.Password;
            tempUser.FirstName = user.FirstName;
            tempUser.LastName = user.LastName;
            tempUser.Phone = user.Phone;
            tempUser.Email = user.Email;
            tempUser.IsReviewer = user.IsReviewer;
            tempUser.IsAdmin = user.IsAdmin;
            tempUser.Active = user.Active;
            tempUser.DateUpdated = DateTime.Now;
            return TrySave("changed.");
        }

        public ActionResult Remove([FromBody] User user)
        {
            User tempUser = db.Users.Find(user.Id);
            db.Users.Remove(tempUser);
            return TrySave("removed.");

        }
    }
}