using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace ECommerce.Pages
{
    public class AddToCartModel : PageModel
    {
        private readonly ILogger<IndexModel> logger;
        private readonly IECommerceDataHazelCast eCommerceData;

        public AddToCartModel(ILogger<IndexModel> logger, IECommerceDataHazelCast eCommerceData)
        {
            this.logger = logger;
            this.eCommerceData = eCommerceData;
        }

        [BindProperty]
        public CartItem CartItem { get; set; }
        [BindProperty]
        public List<Product> Products { get; set; }

        public IActionResult OnGet()
        {
            this.CartItem = new CartItem(0, 1, "🍇", "Grapes box", 3.50m, 1);
            this.Products = eCommerceData.GetProductList();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await eCommerceData.AddCartItemAsync(CartItem);

            return RedirectToPage("Index");
        }

        public JsonResult OnGetCartItem(CartItem cartItem)
        {
            var product = eCommerceData.GetProductList().Where(p => p.Id == cartItem.ProductId).Single();
            var newItem = new CartItem(cartItem.Id, product.Id, product.Icon, product.Description, product.UnitPrice, cartItem.Quantity);
            return new JsonResult(newItem);
        }
    }
}
