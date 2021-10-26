using ECommerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce
{
    public interface IECommerceData : IBaseECommerceData
    {
        void Initialize();
        List<CartItem> GetCartItems();
        void AddCartItem(CartItem cartItem);
        void Checkout();
        List<Order> OrdersAwaitingPayment();
        List<Order> OrdersForDelivery();
        List<Order> OrdersRejected();
        void ApprovePayment();
        void RejectPayment();
    }

    public class ECommerceData : BaseECommerceData, IECommerceData
    {
        private Dictionary<int, CartItem> cartItems;
        private Queue<Order> ordersAwaitingPayment;
        private Queue<Order> ordersForDelivery;
        private Queue<Order> ordersRejected;

        public void Initialize()
        {
            cartItems = new Dictionary<int, CartItem>()
            {
                [17] = new CartItem(1, 17, "🥥", "Coconut", 4.50m, 2),
                [13] = new CartItem(2, 13, "🍒", "Cherries box", 3.50m, 3),
                [4] = new CartItem(3, 4, "🍊", "Tangerine box", 3.50m, 1)
            };

            ordersAwaitingPayment = new Queue<Order>(
                new List<Order>
                {
                    new Order(1006, new DateTime(2021, 10, 11, 3, 3, 0), 7, 70.00m),
                    new Order(1007, new DateTime(2021, 10, 12, 17, 17, 0), 2, 20.00m),
                    new Order(1008, new DateTime(2021, 10, 13, 21, 9, 0), 5, 50.00m)
                });

            ordersForDelivery = new Queue<Order>(
                new List<Order>
                {
                    new Order(1002, new DateTime(2021, 10, 2, 23, 3, 0), 5, 50.00m),
                    new Order(1003, new DateTime(2021, 10, 9, 7, 7, 0), 3, 30.00m)
                });

            ordersRejected = new Queue<Order>(
                new List<Order>
                {
                    new Order(1001, new DateTime(2021, 10, 1, 18, 32, 0), 5, 35.00m),
                    new Order(1004, new DateTime(2021, 10, 3, 17, 17, 0), 2, 24.00m),
                    new Order(1005, new DateTime(2021, 10, 7, 09, 12, 0), 4, 17.00m)
                });

            MaxOrderId = 1008;
        }

        public List<CartItem> GetCartItems()
        {
            return cartItems.OrderBy(i => i.Key).Select(i => i.Value).ToList();
        }

        public void AddCartItem(CartItem cartItem)
        {
            var product = GetProductList().Where(p => p.Id == cartItem.ProductId).Single();
            var newItem = new CartItem(cartItem.Id, product.Id, product.Icon, product.Description, product.UnitPrice, cartItem.Quantity);
            cartItems.Add(newItem.ProductId, newItem);
        }

        public List<Order> OrdersAwaitingPayment()
        {
            return ordersAwaitingPayment.OrderByDescending(o => o.Id).ToList();
        }

        public List<Order> OrdersForDelivery()
        {
            return ordersForDelivery.OrderByDescending(o => o.Id).ToList();
        }

        public List<Order> OrdersRejected()
        {
            return ordersRejected.OrderByDescending(o => o.Id).ToList();
        }

        public void ApprovePayment()
        {
            var order = ordersAwaitingPayment.Dequeue();
            ordersForDelivery.Enqueue(order);
        }

        public void RejectPayment()
        {
            var order = ordersAwaitingPayment.Dequeue();
            ordersRejected.Enqueue(order);
        }

        public void Checkout()
        {
            int orderId = ++MaxOrderId;

            var order = new Order(orderId, DateTime.Now, cartItems.Count, cartItems.Sum(i => i.Value.Quantity * i.Value.UnitPrice));
            ordersAwaitingPayment.Enqueue(order);
            cartItems.Clear();
        }
    }
}
