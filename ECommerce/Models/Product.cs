using System;

namespace ECommerce.Models
{
    [Serializable]
    public class Product : BaseEntity
    {
        public Product()
        {

        }

        public Product(int id, string icon, string description, decimal unitPrice) : base(id)
        {
            Icon = icon;
            Description = description;
            UnitPrice = unitPrice;
        }

        public string Icon { get; set; }
        public string Description { get; set; }
        public decimal UnitPrice { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}, Icon: {Icon}, Description: {Description}, UnitPrice: {UnitPrice:0.##}";
        }
    }

}
