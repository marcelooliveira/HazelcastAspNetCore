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
        private readonly IECommerceData eCommerceData;

        public CartModel(IECommerceData eCommerceData)
        {
            this.eCommerceData = eCommerceData;
        }

        public List<CartItem> CartItems { get; private set; }
        [BindProperty]
        public string addToCartSubmit { get; set; }
        [BindProperty]
        public string checkoutSubmit { get; set; }

        public async Task OnGetAsync()
        {
            await InitializePageAsync();
        }

        private async Task InitializePageAsync()
        {
            //await eCommerceData.InitializeAsync();
            this.CartItems = await eCommerceData.GetCartItemsAsync();
        }

        public async Task<IActionResult> OnPostAsync()
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
                await eCommerceData.CheckoutAsync();
            }

            await InitializePageAsync();
            return Page();
        }
    }
}
