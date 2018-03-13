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
    public class ProductsController : Controller
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
            return Json(new JsonMessage("Success", "Product was " + actionResult));
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
            return new JsonNetResult { Data = db.Products.ToList() };
        }

        public ActionResult Get(int? id)
        {
            if (id == null)
            {
                return Json(new JsonMessage("Failure", "Id is null"), JsonRequestBehavior.AllowGet);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return Json(new JsonMessage("Failure", "Product does not exist. Do you have the correct Id?"), JsonRequestBehavior.AllowGet);
            }
            return new JsonNetResult { Data = product };
        }

        // [POST] /Customers/Create
        public ActionResult Create([FromBody] Product product)
        {
            product.Active = true;
            product.DateCreated = DateTime.Now;
            if (!ModelState.IsValid)
            {
                return Js(new JsonMessage("Failure", "ModelState is not valid"));
            }
            db.Products.Add(product);
            return TrySave("created.");

        }

        // [POST] /Customers/Change
        public ActionResult Change([FromBody] Product product)
        {
            Product tempProduct = db.Products.Find(product.Id);
            if (tempProduct == null)
            {
                return Js(new JsonMessage("Failure", "Record of product to be changed does not exist"));
            }
            tempProduct.VendorId = product.VendorId;
            tempProduct.PartNumber = product.PartNumber;
            tempProduct.Name = product.Name;
            tempProduct.Price = product.Price;
            tempProduct.Unit = product.Unit;
            tempProduct.PhotoPath = product.PhotoPath;
            tempProduct.Active = product.Active;            
            tempProduct.DateUpdated = DateTime.Now;
            return TrySave("changed.");
        }

        public ActionResult Remove([FromBody] Product product)
        {
            Product tempProduct = db.Products.Find(product.Id);
            db.Products.Remove(tempProduct);
            return TrySave("removed.");

        }
    }
}
