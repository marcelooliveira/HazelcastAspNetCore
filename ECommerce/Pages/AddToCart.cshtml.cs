﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ECommerce.Pages
{
    public class AddToCartModel : PageModel
    {
        private readonly IECommerceData eCommerceData;

        public AddToCartModel(IECommerceData eCommerceData)
        {
            this.eCommerceData = eCommerceData;
        }

        [BindProperty]
        public CartItem CartItem { get; set; }
        [BindProperty]
        public List<Product> Products { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            //await eCommerceData.InitializeAsync();
            this.CartItem = new CartItem(0, 1, "🍇", "Grapes box", 3.50m, 1);
            this.Products = eCommerceData.GetProductList();
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            eCommerceData.AddCartItemAsync(CartItem);

            return RedirectToPage("Cart");
        }

        public JsonResult OnGetCartItem(CartItem cartItem)
        {
            var product = eCommerceData.GetProductList().Where(p => p.Id == cartItem.ProductId).Single();
            var newItem = new CartItem(cartItem.Id, product.Id, product.Icon, product.Description, product.UnitPrice, cartItem.Quantity);
            return new JsonResult(newItem);
        }
    }
}