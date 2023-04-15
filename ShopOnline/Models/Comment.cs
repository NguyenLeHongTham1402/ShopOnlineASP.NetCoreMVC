using System;
using System.Collections.Generic;

#nullable disable

namespace ShopOnline.Models
{
    public partial class Comment
    {
        public Comment()
        {
            InverseParent = new HashSet<Comment>();
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int? ProductId { get; set; }
        public int? ParentId { get; set; }
        public bool IsActive { get; set; }

        public DateTime? CreatedDate { get; set; }

        public virtual Comment Parent { get; set; }
        public virtual Product Product { get; set; }
        public virtual User UsernameNavigation { get; set; }
        public virtual ICollection<Comment> InverseParent { get; set; }
    }
}
