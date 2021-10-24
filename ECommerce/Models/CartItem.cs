using System;

namespace ECommerce.Models
{
    [Serializable]
    public class CartItem : BaseEntity
    {
        public CartItem()
        {

        }

        public CartItem(int id, int productId, string icon, string description, decimal unitPrice, int quantity)
            : base(id)
        {
            ProductId = productId;
            Icon = icon;
            Description = description;
            UnitPrice = unitPrice;
            Quantity = quantity;
        }

        public int ProductId { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get { return Quantity * UnitPrice; } } 
    }

}
