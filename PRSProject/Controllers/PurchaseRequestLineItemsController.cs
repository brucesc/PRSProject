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
    public class PurchaseRequestLineItemsController : Controller
    {
        PRSDbContext db = new PRSDbContext();


        public decimal CalculateTotal(PurchaseRequestLineItem prli)
        {
            PurchaseRequest purchaseRequest = db.PurchaseRequests.Find(prli.PurchaseRequestId);
            Product product = db.Products.Find(prli.ProductId);
            decimal total = product.Price * prli.Quantity;
            List<PurchaseRequestLineItem> purchaseRequestLineItems = db.PurchaseRequestLineItems.
                Where(p => p.PurchaseRequestId == purchaseRequest.Id).ToList();

            foreach (PurchaseRequestLineItem item in purchaseRequestLineItems)
            {
                total += item.Product.Price * item.Quantity;
            }
            return total;
        }

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
            return Json(db.PurchaseRequestLineItems.ToList(), JsonRequestBehavior.AllowGet);
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
            return Json(purchaseRequestLineItem, JsonRequestBehavior.AllowGet);
        }

        // [POST] /Customers/Create
        public ActionResult Create([FromBody] PurchaseRequestLineItem purchaseRequestLineItem)
        {
            if (!ModelState.IsValid)
            {
                return Js(new JsonMessage("Failure", "ModelState is not valid"));
            }
            db.PurchaseRequestLineItems.Add(purchaseRequestLineItem);
            
            var MakeCalculation = CalculateTotal(purchaseRequestLineItem);
            purchaseRequestLineItem.PurchaseRequest.Total = MakeCalculation;
           

            return TrySave("created.");
        }

        // [POST] /Customers/Change
        public ActionResult Change([FromBody] PurchaseRequestLineItem purchaseRequestLineItem)
        {
            PurchaseRequestLineItem tempPurchaseRequestLineItem = db.PurchaseRequestLineItems.Find(purchaseRequestLineItem.Id);
            if (tempPurchaseRequestLineItem == null)
            {
                return Js(new JsonMessage("Failure", "Record of Purchase Request Line Item to be changed does not exist"));
            }
            tempPurchaseRequestLineItem.PurchaseRequestId = purchaseRequestLineItem.PurchaseRequestId;
            tempPurchaseRequestLineItem.ProductId = purchaseRequestLineItem.ProductId;
            tempPurchaseRequestLineItem.Quantity = purchaseRequestLineItem.Quantity;

            var MakeCalculation = CalculateTotal(tempPurchaseRequestLineItem);
            tempPurchaseRequestLineItem.PurchaseRequest.Total = MakeCalculation;

            return TrySave("changed.");
        }

        public ActionResult Remove([FromBody] PurchaseRequestLineItem purchaseRequestLineItem)
        {
            PurchaseRequestLineItem tempPurchaseRequestLineItem = db.PurchaseRequestLineItems.Find(purchaseRequestLineItem.Id);
            db.PurchaseRequestLineItems.Remove(tempPurchaseRequestLineItem);
            return TrySave("removed.");

        }
    }
}