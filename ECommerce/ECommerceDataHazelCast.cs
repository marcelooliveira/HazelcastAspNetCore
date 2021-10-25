using ECommerce.Models;
using Hazelcast;
using Hazelcast.DistributedObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce
{
    public class ECommerceDataHazelCast : IECommerceData
    {
        private IHMap<int, CartItem> cartItemsMap;
        private IHQueue<Order> ordersAwaitingPaymentQueue;
        private List<Order> ordersForDelivery;
        private List<Order> ordersRejected;
        public int MaxOrderId { get; private set; }

        public IHazelcastClient hazelcastClient { get; private set; }

        public async Task InitializeAsync(IHazelcastClient hazelcastClient)
        {
            this.hazelcastClient = hazelcastClient;

            // Get the Distributed Map from Cluster.
            cartItemsMap = await hazelcastClient.GetMapAsync<int, CartItem>("distributed-cartitem-map");
            await cartItemsMap.PutAsync(17, new CartItem(1, 17, "🥥", "Coconut", 4.50m, 2));
            await cartItemsMap.PutAsync(13, new CartItem(2, 13, "🍒", "Cherries box", 3.50m, 3));
            await cartItemsMap.PutAsync(4, new CartItem(3, 4, "🍊", "Tangerine box", 3.50m, 1));

            ordersAwaitingPaymentQueue = await hazelcastClient.GetQueueAsync<Order>("distributed-order-queue");
            await ordersAwaitingPaymentQueue.PutAsync(new Order(1006, new DateTime(2021, 10, 11, 3, 3, 0), 7, 70.00m));
            await ordersAwaitingPaymentQueue.PutAsync(new Order(1007, new DateTime(2021, 10, 12, 17, 17, 0), 2, 20.00m));
            await ordersAwaitingPaymentQueue.PutAsync(new Order(1008, new DateTime(2021, 10, 13, 21, 9, 0), 5, 50.00m));

            ordersForDelivery = new List<Order>
                {
                    new Order(1002, new DateTime(2021, 10, 2, 23, 3, 0), 5, 50.00m),
                    new Order(1003, new DateTime(2021, 10, 9, 7, 7, 0), 3, 30.00m)
                };

            ordersRejected = new List<Order>
                {
                    new Order(1001, new DateTime(2021, 10, 1, 18, 32, 0), 5, 35.00m),
                    new Order(1004, new DateTime(2021, 10, 3, 17, 17, 0), 2, 24.00m),
                    new Order(1005, new DateTime(2021, 10, 7, 09, 12, 0), 4, 17.00m)
                };

            MaxOrderId = 1008;
        }

        public List<Product> GetProductList()
        {
            return new List<Product>()
            {
                new Product ( 1, "🍇", "Grapes box", 3.50m ),
                new Product ( 2, "🍈", "Melon box", 3.50m ),
                new Product ( 3, "🍉", "Watermelon box", 5.50m ),
                new Product ( 4, "🍊", "Tangerine box", 3.50m ),
                new Product ( 5, "🍋", "Lemon box", 3.50m ),
                new Product ( 6, "🍌", "Banana box", 3.50m ),
                new Product ( 7, "🍍", "Pineapple box", 3.50m ),
                new Product ( 8, "🥭", "Mango box", 4.50m ),
                new Product ( 9, "🍎", "Red Apple box", 3.50m ),
                new Product ( 10, "🍏", "Green Apple box", 6.50m ),
                new Product ( 11, "🍐", "Pear box", 3.50m ),
                new Product ( 12, "🍑", "Peach box", 3.50m ),
                new Product ( 13, "🍒", "Cherries box", 3.50m ),
                new Product ( 14, "🍓", "Strawberry box", 3.50m ),
                new Product ( 15, "🥝", "Kiwi Fruit box", 7.50m ),
                new Product ( 16, "🍅", "Tomato box", 2.50m ),
                new Product ( 17, "🥥", "Coconut", 4.50m )
            };
        }

        public async Task<List<CartItem>> GetCartItemsAsync()
        {
            var values = await cartItemsMap.GetValuesAsync();
            return values.OrderBy(i => i.Id).ToList();
        }

        public async Task AddCartItemAsync(CartItem cartItem)
        {
            var product = GetProductList().Where(p => p.Id == cartItem.ProductId).Single();
            var newItem = new CartItem(cartItem.Id, product.Id, product.Icon, product.Description, product.UnitPrice, cartItem.Quantity);
            await cartItemsMap.PutAsync(newItem.ProductId, newItem);
        }

        public async Task<List<Order>> OrdersAwaitingPaymentAsync()
        {
            var list = await ordersAwaitingPaymentQueue.GetAllAsync();
            return list.ToList().OrderByDescending(o => o.Id).ToList();
        }

        public List<Order> OrdersForDelivery()
        {
            return ordersForDelivery.OrderByDescending(o => o.Id).ToList();
        }

        public List<Order> OrdersRejected()
        {
            return ordersRejected.OrderByDescending(o => o.Id).ToList();
        }

        public async Task ApprovePaymentAsync()
        {
            var order = await ordersAwaitingPaymentQueue.TakeAsync();
            ordersForDelivery.Add(order);
        }

        public async Task RejectPaymentAsync()
        {
            var order = await ordersAwaitingPaymentQueue.TakeAsync();
            ordersRejected.Add(order);
        }

        public async Task CheckoutAsync()
        {
            int orderId = ++MaxOrderId;

            var cartItems = await cartItemsMap.GetValuesAsync();
            var order = new Order(orderId, DateTime.Now, cartItems.Count, cartItems.Sum(i => i.Quantity * i.UnitPrice));
            await ordersAwaitingPaymentQueue.PutAsync(order);
            await cartItemsMap.ClearAsync();
        }
    }
}
