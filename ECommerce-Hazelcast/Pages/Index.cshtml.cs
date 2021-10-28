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
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> logger;
        private readonly IECommerceDataHazelCast eCommerceData;

        public IndexModel(ILogger<IndexModel> logger, IECommerceDataHazelCast eCommerceData)
        {
            this.logger = logger;
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
