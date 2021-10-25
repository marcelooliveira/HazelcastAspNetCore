using ECommerce.Models;
using System.Collections.Generic;

namespace ECommerce
{
    public interface IECommerceData
    {
        List<CartItem> GetCartItems();
        void AddCartItem(CartItem cartItem);
        void Checkout();
        void ApprovePayment();
        List<Product> GetProductList();
        List<Order> OrdersAwaitingPayment();
        List<Order> OrdersForDelivery();
        List<Order> OrdersRejected();
        void RejectPayment();
    }
}