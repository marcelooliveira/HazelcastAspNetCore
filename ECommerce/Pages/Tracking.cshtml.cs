using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace ECommerce.Pages
{
    public class TrackingModel : PageModel
    {
        private readonly ILogger<IndexModel> logger;
        private readonly IECommerceData eCommerceData;

        public TrackingModel(ILogger<IndexModel> logger, IECommerceData eCommerceData)
        {
            this.logger = logger;
            this.eCommerceData = eCommerceData;
        }

        public List<Order> OrdersForDelivery { get; private set; }
        public List<Order> OrdersRejected { get; private set; }

        public void OnGet()
        {
            InitializePage();
        }

        private void InitializePage()
        {
            this.OrdersForDelivery = eCommerceData.OrdersForDelivery();
            this.OrdersRejected = eCommerceData.OrdersRejected();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            InitializePage();
            return Page();
        }
    }
}
