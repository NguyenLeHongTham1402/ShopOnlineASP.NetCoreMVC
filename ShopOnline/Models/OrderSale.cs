using System;
using System.Collections.Generic;

#nullable disable

namespace ShopOnline.Models
{
    public partial class OrderSale
    {
        public OrderSale()
        {
            Bills = new HashSet<Bill>();
            OrderDtls = new HashSet<OrderDtl>();
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string OrderName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public string Payment { get; set; }
        public string Receiver { get; set; }
        public double TransportFee { get; set; }
        public string Transportion { get; set; }
        public bool? IsPaid { get; set; }
        public string Status { get; set; }
        public bool? IsActive { get; set; }

        public virtual User UsernameNavigation { get; set; }
        public virtual ICollection<Bill> Bills { get; set; }
        public virtual ICollection<OrderDtl> OrderDtls { get; set; }
    }
}
