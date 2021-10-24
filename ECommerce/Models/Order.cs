using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Models
{

    public class Order : BaseEntity
    {
        public Order()
        {

        }

        public Order(int id, DateTime placement, int itemCount, decimal total) : base(id)
        {
            Placement = placement;
            ItemCount = itemCount;
            Total = total;
        }

        public DateTime Placement { get; set; }
        public int ItemCount { get; set; }
        public decimal Total { get; set; }
    }

}
