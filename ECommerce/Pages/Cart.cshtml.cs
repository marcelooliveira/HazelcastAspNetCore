using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ECommerce.Pages
{
    public class CartModel : PageModel
    {
        public List<CartItem> CartItems { get; private set; }
        [BindProperty]
        public string addToCartSubmit { get; set; }
        [BindProperty]
        public string checkoutSubmit { get; set; }

        public void OnGet()
        {
            InitializePage();
        }

        private void InitializePage()
        {
            this.CartItems = ECommerceData.Instance.GetCartItems();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (!string.IsNullOrWhiteSpace(addToCartSubmit))
            {
                return RedirectToPage("AddToCart");
            }

            if (!string.IsNullOrWhiteSpace(checkoutSubmit))
            {
                ECommerceData.Instance.Checkout();
            }

            InitializePage();
            return Page();
        }
    }
}
