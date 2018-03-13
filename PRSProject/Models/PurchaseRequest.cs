using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PRSProject.Models
{
    public class PurchaseRequest
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        [Required]
        [StringLength(100)]
        public string Description { get; set; }
        [Required]
        [StringLength(255)]
        public string Justification { get; set; }
        public DateTime DateNeeded { get; set; }
        [Required]
        [StringLength(25)]
        public string DeliveryMode { get; set; }
        [DefaultValue("New")]
        public string Status { get; set; }
        [DefaultValue(0)]
        public decimal Total { get; set; }
        [DefaultValue(true)]
        public bool Active { get; set; }
        [StringLength(100)]
        public string ReasonForRejection { get; set; }
        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DateCreated { get; set; }
        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? DateUpdated { get; set; }
        //public int UpdatedByUser { get; set; }

        public virtual User User { get; set; }        
        public virtual List<PurchaseRequestLineItem> PurchaseRequestLineItems { get; set; }
    }
}