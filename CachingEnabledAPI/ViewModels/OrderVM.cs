using CachingEnabledAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CachingEnabledAPI.ViewModels
{
    public class OrderVM: Order
    {
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
    }
}
