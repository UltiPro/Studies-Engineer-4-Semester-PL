#nullable disable
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using efcore.Models;
using efcore.DAL;

namespace efcore.Pages.Categories
{
    public class ShowModel : PageModel
    {
        CatDAL cdl;
        public List<Product> ListOfProducts { get; set; }
        public void OnGet(int? id)
        {
            cdl = new CatDAL();
            ListOfProducts = cdl.List((int)id);
        }
    }
}