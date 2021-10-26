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
        private readonly IECommerceData eCommerceData;

        public PaymentModel(IECommerceData eCommerceData)
        {
            this.eCommerceData = eCommerceData;
        }

        public List<Order> OrdersAwaitingPayment { get; private set; }

        [BindProperty]
        public string approveSubmit { get; set; }
        [BindProperty]
        public string rejectSubmit { get; set; }

        public void OnGet()
        {
            InitializePage();
        }

        private void InitializePage()
        {
            this.OrdersAwaitingPayment = eCommerceData.OrdersAwaitingPayment();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (!string.IsNullOrWhiteSpace(approveSubmit))
            {
                eCommerceData.ApprovePayment();
            }

            if (!string.IsNullOrWhiteSpace(rejectSubmit))
            {
                eCommerceData.RejectPayment();
            }

            InitializePage();
            return Page();
        }
    }
}
