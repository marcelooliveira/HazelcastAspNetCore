using ECommerce.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce
{
    public interface IECommerceData
    {
        Task InitializeAsync(Hazelcast.IHazelcastClient client);
        Task<List<CartItem>> GetCartItemsAsync();
        Task AddCartItemAsync(CartItem cartItem);
        Task CheckoutAsync();
        void ApprovePayment();
        List<Product> GetProductList();
        List<Order> OrdersAwaitingPayment();
        List<Order> OrdersForDelivery();
        List<Order> OrdersRejected();
        void RejectPayment();
    }
}