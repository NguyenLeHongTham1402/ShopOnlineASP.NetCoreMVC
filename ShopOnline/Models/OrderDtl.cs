using System;
using System.Collections.Generic;

#nullable disable

namespace ShopOnline.Models
{
    public partial class OrderDtl
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int? Quantity { get; set; }
        public double? Price { get; set; }
        public double? Total { get; set; }

        public virtual OrderSale Order { get; set; }

        
        public virtual Product Product { get; set; }

        public OrderDtl()
        {

        }

        public OrderDtl(int OrderId, int Productid, int quantity, double price, double total)
        {
            this.OrderId = OrderId;
            this.ProductId = Productid;
            this.Quantity = quantity;
            this.Price = price;
            this.Total = total;
        }
    }
}
