using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using efcore.Data;
using efcore.Models;

namespace efcore.Pages.CategoriesProducts
{
    public class EditModel : PageModel
    {
        private readonly efcore.Data.ShopContext _context;

        public EditModel(efcore.Data.ShopContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ProductCategory ProductCategory { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.ProductCategory == null)
            {
                return NotFound();
            }

            var productcategory = await _context.ProductCategory.FirstOrDefaultAsync(m => m.id == id);
            if (productcategory == null)
            {
                return NotFound();
            }
            ProductCategory = productcategory;
            ViewData["CategoryId"] = new SelectList(_context.Category, "ID", "name");
            ViewData["ProductId"] = new SelectList(_context.Product, "id", "name");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            /*if (!ModelState.IsValid)
            {
                return Page();
            }*/

            _context.Attach(ProductCategory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductCategoryExists(ProductCategory.id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ProductCategoryExists(int id)
        {
            return (_context.ProductCategory?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}