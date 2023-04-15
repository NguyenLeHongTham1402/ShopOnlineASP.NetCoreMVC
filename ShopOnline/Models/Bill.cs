using System;
using System.Collections.Generic;

#nullable disable

namespace ShopOnline.Models
{
    public partial class Bill
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public double? Total { get; set; }
        public string Status { get; set; }
        public string Payment { get; set; }
        public string Payer { get; set; }

        public virtual OrderSale Order { get; set; }
    }
}
