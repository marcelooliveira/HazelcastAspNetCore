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
        List<Product> GetProductList();
        Task<List<Order>> OrdersAwaitingPaymentAsync();
        List<Order> OrdersForDelivery();
        List<Order> OrdersRejected();
        Task ApprovePaymentAsync();
        Task RejectPaymentAsync();
    }
}