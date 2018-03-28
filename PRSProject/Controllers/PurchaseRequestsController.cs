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
    public class PurchaseRequestsController : Controller
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
            return Json(new JsonMessage("Success", "Purchase Request was " + actionResult));
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
            return new JsonNetResult { Data = db.PurchaseRequests.ToList() };
        }

        public ActionResult Get(int? id)
        {
            if (id == null)
            {
                return Json(new JsonMessage("Failure", "Id is null"), JsonRequestBehavior.AllowGet);
            }
            PurchaseRequest purchaseRequest = db.PurchaseRequests.Find(id);
            if (purchaseRequest == null)
            {
                return Json(new JsonMessage("Failure", "Purchase Request does not exist. Do you have the correct Id?"), JsonRequestBehavior.AllowGet);
            }
            return new JsonNetResult { Data = purchaseRequest };
        }

        // [POST] /PurchaseRequests/Create
        public ActionResult Create([FromBody] PurchaseRequest purchaseRequest)
        {
            purchaseRequest.Total = 0;
            purchaseRequest.Active = true;
            purchaseRequest.DateCreated = DateTime.Now;
            if (!ModelState.IsValid)
            {
                return Js(new JsonMessage("Failure", "ModelState is not valid"));
            }
            db.PurchaseRequests.Add(purchaseRequest);
            return TrySave("created.");

        }

        // [POST] /PurchaseRequests/Change
        public ActionResult Change([FromBody] PurchaseRequest purchaseRequest)
        {
            if (purchaseRequest.Description == null) return new EmptyResult();

            PurchaseRequest tempPurchaseRequest = db.PurchaseRequests.Find(purchaseRequest.Id);
            if (tempPurchaseRequest == null)
            {
                return Js(new JsonMessage("Failure", "Record of Purchase Request to be changed does not exist"));
            }
            tempPurchaseRequest.UserId = purchaseRequest.UserId;
            tempPurchaseRequest.Description = purchaseRequest.Description;
            tempPurchaseRequest.Justification = purchaseRequest.Justification;
            tempPurchaseRequest.DateNeeded = purchaseRequest.DateNeeded;
            tempPurchaseRequest.DeliveryMode = purchaseRequest.DeliveryMode;
            tempPurchaseRequest.Status = purchaseRequest.Status;            
            tempPurchaseRequest.Active = purchaseRequest.Active;
            tempPurchaseRequest.ReasonForRejection = purchaseRequest.ReasonForRejection;            
            tempPurchaseRequest.DateUpdated = DateTime.Now;
            return TrySave("changed.");
        }

        public ActionResult Remove([FromBody] PurchaseRequest purchaseRequest)
        {
            if (purchaseRequest.Description == null) return new EmptyResult();
            PurchaseRequest tempPurchaseRequest = db.PurchaseRequests.Find(purchaseRequest.Id);
            db.PurchaseRequests.Remove(tempPurchaseRequest);
            return TrySave("removed.");

        }
    }
}
