using System;
using System.Collections.Generic;

namespace ProjetNet.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public List<OrderDetail> OrderDetails { get; set; }
    }
}