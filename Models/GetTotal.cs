using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppPerfume.Models
{
    public class GetTotal
    {
        public decimal TotalAmount { get; set; }
        public List<CART> CartItems { get; set; }
    }
}