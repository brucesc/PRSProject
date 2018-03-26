using PRSProject.Models;
using PRSProject.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Utility;

namespace PRSProject.Controllers
{
    public class VendorsController : Controller
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
            return Json(new JsonMessage("Success", "Vendor was " + actionResult));
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
            return new JsonNetResult { Data = db.Vendors.ToList() };
        }

        public ActionResult Get(int? id)
        {
            if (id == null)
            {
                return Json(new JsonMessage("Failure", "Id is null"), JsonRequestBehavior.AllowGet);
            }
            Vendor vendor = db.Vendors.Find(id);
            if (vendor == null)
            {
                return Json(new JsonMessage("Failure", "Vendor does not exist. Do you have the correct Id?"), JsonRequestBehavior.AllowGet);
            }
            return new JsonNetResult { Data = vendor };
        }

        // [POST] /Customers/Create
        public ActionResult Create([FromBody] Vendor vendor)
        {
            vendor.Active = true;
            vendor.DateCreated = DateTime.Now;
            if (!ModelState.IsValid)
            {
                return Js(new JsonMessage("Failure", "ModelState is not valid"));
            }
            db.Vendors.Add(vendor);
            return TrySave("created.");

        }

        // [POST] /Customers/Change
        public ActionResult Change([FromBody] Vendor vendor)
        {
            if (vendor.Name == null) return new EmptyResult();
            Vendor tempVendor = db.Vendors.Find(vendor.Id);
            if (tempVendor == null)
            {
                return Js(new JsonMessage("Failure", "Record of vendor to be changed does not exist"));
            }
            tempVendor.Code = vendor.Code;
            tempVendor.Name = vendor.Name;
            tempVendor.Address = vendor.Address;
            tempVendor.City = vendor.City;
            tempVendor.State = vendor.State;
            tempVendor.Zip = vendor.Zip;
            tempVendor.Phone = vendor.Phone;
            tempVendor.Email = vendor.Email;
            tempVendor.IsPreApproved = vendor.IsPreApproved;
            tempVendor.Active = vendor.Active;            
            tempVendor.DateUpdated = DateTime.Now;
            return TrySave("changed.");
        }

        public ActionResult Remove([FromBody] Vendor vendor)
        {
            if (vendor.Name == null) return new EmptyResult();
            Vendor tempVendor = db.Vendors.Find(vendor.Id);
            db.Vendors.Remove(tempVendor);
            return TrySave("removed.");

        }
    }
}