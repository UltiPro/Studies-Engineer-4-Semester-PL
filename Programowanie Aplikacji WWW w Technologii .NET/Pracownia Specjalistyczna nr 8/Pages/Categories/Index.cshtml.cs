using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using efcore.Data;
using efcore.Models;

namespace efcore.Pages.Categories
{
    public class IndexModel : PageModel
    {
        private readonly efcore.Data.ShopContext _context;

        public IndexModel(efcore.Data.ShopContext context)
        {
            _context = context;
        }

        public IList<Category> Category { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Category != null)
            {
                Category = await _context.Category.ToListAsync();
            }
        }
    }
}