using System;
using System.Collections.Generic;

#nullable disable

namespace ShopOnline.Models
{
    public partial class User
    {
        public User()
        {
            Comments = new HashSet<Comment>();
            OrderSales = new HashSet<OrderSale>();
        }

        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public string Role { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<OrderSale> OrderSales { get; set; }
    }
}
