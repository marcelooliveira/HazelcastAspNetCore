using Hazelcast;
using Hazelcast.DistributedObjects;
using Hazelcast.Serialization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ConsolePoC
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            var products = GetProductList();

            var options = HazelcastOptions.Build();
            // create an Hazelcast client and connect to a server running on localhost
            var hazelClient = await HazelcastClientFactory.StartNewClientAsync(options);

            #region MAP
            // Get the Distributed Map from Cluster.
            var productMap = await hazelClient.GetMapAsync<int, Product>("product-map");
            await PutAllProducts(products, productMap);
            await GetAllProducts(productMap);

            ////Concurrent Map methods, optimistic updating
            //await productMap.PutIfAbsentAsync("somekey", "somevalue");
            //await productMap.ReplaceAsync("key", "value", "newvalue");

            // destroy the map
            await hazelClient.DestroyAsync(productMap);
            #endregion

            #region QUEUE
            // Get a Blocking Queue called "my-distributed-queue"
            var queue = await hazelClient.GetQueueAsync<string>("my-distributed-queue");
            // Offer a String into the Distributed Queue
            await queue.OfferAsync("item");
            // Poll the Distributed Queue and return the String
            await queue.PollAsync();
            //Timed blocking Operations
            await queue.OfferAsync("anotheritem", TimeSpan.FromMilliseconds(500));
            await queue.PollAsync(TimeSpan.FromSeconds(5));
            //Indefinitely blocking Operations
            await queue.PutAsync("yetanotheritem");
            var valueFromQueue = await queue.TakeAsync();
            Console.WriteLine($"queueValue: {valueFromQueue}");
            // destroy the queue
            await hazelClient.DestroyAsync(queue);
            #endregion

            Console.WriteLine("Type enter to finish.");
            Console.ReadLine();
        }

        private static async Task GetAllProducts(IHMap<int, Product> productMap)
        {
            for (int i = 0; i < 17; i++)
            {
                //Standard Get.
                var product = await productMap.GetAsync(i + 1);
                Console.WriteLine(product);
            }
        }

        private static List<Product> GetProductList()
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

        private static async Task PutAllProducts(List<Product> products, IHMap<int, Product> productMap)
        {
            foreach (var product in products)
            {
                //Standard Put.
                await productMap.PutAsync(product.Id, product);
            }
        }
    }

    [Serializable]
    public class BaseEntity
    {
        public int Id { get; set; }
    }

    [Serializable]
    public class Product : BaseEntity
    {
        public Product() { }

        public Product(int id, string icon, string description, decimal unitPrice)
        {
            Id = id;
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

    [Serializable]
    public class CartItem : BaseEntity
    {
        public int ProductId { get; set; }
        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }

    public class Order : BaseEntity
    {
        public DateTime Placement { get; set; }
        public int ItemCount { get; set; }
        public decimal Total { get; set; }
        public bool Shipped { get; set; }
    }
}
