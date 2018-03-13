using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace PRSProject.Models
{
    public class Product
    {
        public int Id { get; set; }
        public int VendorId { get; set; }
        [Required]
        [StringLength(50)]
        public string PartNumber { get; set; }
        [Required]
        [StringLength(150)]
        public string Name { get; set; }        
        public decimal Price { get; set; }
        [Required]
        [StringLength(255)]
        public string Unit { get; set; }
        [StringLength(255)]
        public string PhotoPath { get; set; }
        [DefaultValue(true)]
        public bool Active { get; set; }
        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime DateCreated { get; set; }
        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime? DateUpdated { get; set; }
        //public int UpdatedByUser { get; set; }

        public virtual Vendor vendor { get; set; }
    }
}