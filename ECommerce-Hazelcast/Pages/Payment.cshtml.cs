using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ECommerce.Pages
{
    public class PaymentModel : PageModel
    {
        private readonly IECommerceDataHazelCast eCommerceData;

        public PaymentModel(IECommerceDataHazelCast eCommerceData)
        {
            this.eCommerceData = eCommerceData;
        }

        public List<Order> OrdersAwaitingPayment { get; private set; }

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
