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
        private readonly IECommerceDataHazelCast eCommerceData;

        public TrackingModel(ILogger<IndexModel> logger, IECommerceDataHazelCast eCommerceData)
        {
            this.logger = logger;
            this.eCommerceData = eCommerceData;
        }

        public List<Order> OrdersForDelivery { get; private set; }
        public List<Order> OrdersRejected { get; private set; }
        [BindProperty]
        public string approveSubmit { get; set; }
        [BindProperty]
        public string rejectSubmit { get; set; }

        public async Task OnGetAsync()
        {
            await InitializePageAsync();
        }

        private async Task InitializePageAsync()
        {
            this.OrdersForDelivery = await eCommerceData.OrdersForDeliveryAsync();
            this.OrdersRejected = await eCommerceData.OrdersRejectedAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (!string.IsNullOrWhiteSpace(approveSubmit))
            {
                await eCommerceData.ApprovePaymentAsync();
            }

            if (!string.IsNullOrWhiteSpace(rejectSubmit))
            {
                await eCommerceData.RejectPaymentAsync();
            }

            await InitializePageAsync();
            return Page();
        }
    }
}
