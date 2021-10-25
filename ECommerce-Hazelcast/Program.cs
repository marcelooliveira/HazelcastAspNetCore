using Hazelcast;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var webHost = CreateHostBuilder(args).Build();
            await SetupHazelCast(webHost);
            webHost.Run();
        }

        private static async Task SetupHazelCast(IHost webHost)
        {
            var options = HazelcastOptions.Build();
            // create an Hazelcast client and connect to a server running on localhost
            var client = HazelcastClientFactory.StartNewClientAsync(options).Result;
            await webHost.Services.GetRequiredService<IECommerceDataHazelCast>().InitializeAsync(client);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}