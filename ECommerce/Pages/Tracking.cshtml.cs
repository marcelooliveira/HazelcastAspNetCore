using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ECommerce.Pages
{
    public class TrackingModel : PageModel
    {
        private readonly IECommerceData eCommerceData;

        public TrackingModel(IECommerceData eCommerceData)
        {
            this.eCommerceData = eCommerceData;
        }

        public List<Order> OrdersAwaitingPayment { get; private set; }
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
            this.OrdersAwaitingPayment = await eCommerceData.OrdersAwaitingPaymentAsync();
            this.OrdersForDelivery = eCommerceData.OrdersForDelivery();
            this.OrdersRejected = eCommerceData.OrdersRejected();
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
