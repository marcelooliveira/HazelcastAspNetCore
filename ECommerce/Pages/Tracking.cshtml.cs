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
        public List<Order> OrdersAwaitingPayment { get; private set; }
        public List<Order> OrdersForDelivery { get; private set; }
        public List<Order> OrdersRejected { get; private set; }
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
            this.OrdersAwaitingPayment = ECommerceData.Instance.OrdersAwaitingPayment();
            this.OrdersForDelivery = ECommerceData.Instance.OrdersForDelivery();
            this.OrdersRejected = ECommerceData.Instance.OrdersRejected();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (!string.IsNullOrWhiteSpace(approveSubmit))
            {
                ECommerceData.Instance.ApprovePayment();
            }

            if (!string.IsNullOrWhiteSpace(rejectSubmit))
            {
                ECommerceData.Instance.RejectPayment();
            }

            InitializePage();
            return Page();
        }
    }
}
