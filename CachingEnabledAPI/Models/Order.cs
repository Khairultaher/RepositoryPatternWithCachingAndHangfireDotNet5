
using CachingEnabledAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CachingEnabledAPI.Models
{
    public class Order: Auditable
    {
        public int CustomerId { get; set; }
        public long Quantity { get; set; }
        public decimal Price { get; set; }

        //[Column(TypeName = "decimal(18,4)")]
        public decimal Total { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
