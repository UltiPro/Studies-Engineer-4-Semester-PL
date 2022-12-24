#pragma warning disable

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Xml;
using Models.Product;
using DAL.Product2;

namespace WebApplication1
{
    public class EditModel : PageModel
    {
        [BindProperty]
        public Product product { get; set; }
        IProductDB productDB;
        public EditModel(IProductDB _productDB)
        {
            productDB = _productDB;
        }
        public void OnGet(int? id)
        {
            product = productDB.Get((int)id);
        }

        public IActionResult OnPost()
        {
            productDB.Update(product);
            return RedirectToPage("Index");
        }
    }
}
