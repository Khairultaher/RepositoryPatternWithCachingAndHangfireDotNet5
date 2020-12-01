using CachingEnabledAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CachingEnabledAPI.ViewModels
{
    public class CustomerVM: Customer
    {
        public int? OrderId { get; set; }
        public DateTime? OrderDate { get; set; }
    }
}
