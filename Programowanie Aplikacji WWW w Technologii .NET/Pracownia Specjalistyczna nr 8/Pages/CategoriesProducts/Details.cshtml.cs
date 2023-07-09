using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using efcore.Data;
using efcore.Models;
using efcore.DAL;

namespace efcore.Pages.CategoriesProducts
{
    public class DetailsModel : PageModel
    {
        private readonly efcore.Data.ShopContext _context;
        public string CategoryName;
        public string ProductName;

        public DetailsModel(efcore.Data.ShopContext context)
        {
            _context = context;
        }

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
            else
            {
                ProductCategory = productcategory;
            }

            CatProDAL cpd = new CatProDAL();

            CategoryName = cpd.Category((int)id);
            ProductName = cpd.Product((int)id);

            return Page();
        }
    }
}