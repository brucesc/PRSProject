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
    public class PurchaseRequestLineItemsController : Controller
    {
        PRSDbContext db = new PRSDbContext();

        //TODO: Compound Linq statement in place of foreach loop
        public decimal CalculateTotal(PurchaseRequestLineItem prli)
        {
            db = new PRSDbContext(); // refresh the context
            var purchaseRequest = db.PurchaseRequests.Find(prli.PurchaseRequestId);            
            
            purchaseRequest.Total = purchaseRequest.PurchaseRequestLineItems.Sum(p => p.Product.Price * p.Quantity); // Using the virtual List<PurchaseRequestLineItems> we don't need a .Where(linq expression)
                        
            return purchaseRequest.Total;            
        }

        //public decimal CalculateTotal(PurchaseRequestLineItem prli) // Old Version with foreach loop
        //{
        //    db = new PRSDbContext(); // refresh the context
        //    PurchaseRequest purchaseRequest = db.PurchaseRequests.Find(prli.PurchaseRequestId);
        //    purchaseRequest.Total = 0;
        //    List<PurchaseRequestLineItem> purchaseRequestLineItems = db.PurchaseRequestLineItems.Where(p => p.PurchaseRequestId == purchaseRequest.Id).ToList();
        //    foreach (var item in purchaseRequestLineItems)
        //    {
        //        purchaseRequest.Total += item.Product.Price * item.Quantity;
        //    }
        //    return purchaseRequest.Total;
        //}

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
            return Json(new JsonMessage("Success", "Purchase Request Line Item was " + actionResult));
        }
        
        public ActionResult List()
        {
            return new JsonNetResult { Data = db.PurchaseRequestLineItems.ToList() };           
        }

        public ActionResult Get(int? id)
        {
            if (id == null)
            {
                return Json(new JsonMessage("Failure", "Id is null"), JsonRequestBehavior.AllowGet);
            }
            PurchaseRequestLineItem purchaseRequestLineItem = db.PurchaseRequestLineItems.Find(id);
            if (purchaseRequestLineItem == null)
            {
                return Json(new JsonMessage("Failure", "Purchase Request Line Item does not exist. Do you have the correct Id?"), JsonRequestBehavior.AllowGet);
            }
            return new JsonNetResult { Data = purchaseRequestLineItem };
        }

        // [POST] /PurchaseRequestLineItems/Create
        public ActionResult Create([FromBody] PurchaseRequestLineItem purchaseRequestLineItem)
        {
            if (!ModelState.IsValid)
            {
                return Js(new JsonMessage("Failure", "ModelState is not valid"));
            }
            db.PurchaseRequestLineItems.Add(purchaseRequestLineItem);

            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json(new JsonMessage("Failure", ex.Message), JsonRequestBehavior.AllowGet);
            }

            CalculateTotal(purchaseRequestLineItem);
            
            db.SaveChanges();

            return Json(new JsonMessage("Success", "Purchase Request Line Item was created"));
        }

        // [POST] /PurchaseRequestLineItems/Change
        public ActionResult Change([FromBody] PurchaseRequestLineItem purchaseRequestLineItem)
        {
            if (purchaseRequestLineItem.Product.Name == null) return new EmptyResult();

            PurchaseRequestLineItem tempPurchaseRequestLineItem = db.PurchaseRequestLineItems.Find(purchaseRequestLineItem.Id);
            if (tempPurchaseRequestLineItem == null)
            {
                return Js(new JsonMessage("Failure", "Record of Purchase Request Line Item to be changed does not exist"));
            }
            tempPurchaseRequestLineItem.PurchaseRequestId = purchaseRequestLineItem.PurchaseRequestId;
            tempPurchaseRequestLineItem.ProductId = purchaseRequestLineItem.ProductId;
            tempPurchaseRequestLineItem.Quantity = purchaseRequestLineItem.Quantity;

            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                return Json(new JsonMessage("Failure", ex.Message), JsonRequestBehavior.AllowGet);
            }

            CalculateTotal(tempPurchaseRequestLineItem);
            
            return TrySave("changed.");
        }

        public ActionResult Remove([FromBody] PurchaseRequestLineItem purchaseRequestLineItem)
        {
            if (purchaseRequestLineItem.Product.Name == null) return new EmptyResult();

            PurchaseRequestLineItem tempPurchaseRequestLineItem = db.PurchaseRequestLineItems.Find(purchaseRequestLineItem.Id);

            db.PurchaseRequestLineItems.Remove(tempPurchaseRequestLineItem);
            TrySave("removed.");
            CalculateTotal(purchaseRequestLineItem);
            return TrySave("removed.");

        }
    }
}