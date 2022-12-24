using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using efcore.Data;
using efcore.Models;

namespace efcore.Pages.CategoriesProducts
{
    public class CreateModel : PageModel
    {
        private readonly efcore.Data.ShopContext _context;

        public CreateModel(efcore.Data.ShopContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["CategoryId"] = new SelectList(_context.Category, "ID", "name");
        ViewData["ProductId"] = new SelectList(_context.Product, "id", "name");
            return Page();
        }

        [BindProperty]
        public ProductCategory ProductCategory { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          /*if (!ModelState.IsValid || _context.ProductCategory == null || ProductCategory == null)
            {
                return Page();
            }*/

            _context.ProductCategory.Add(ProductCategory);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
