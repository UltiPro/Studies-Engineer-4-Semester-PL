#pragma warning disable

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Xml;
using Models.Product;
using DAL.Product2;

namespace Pracownia_Specjalistyczna_nr_9.Pages;

public class DetailsModel : PageModel
{
    IProductDB productDB;
    public DetailsModel(IProductDB _productDB)
    {
        productDB = _productDB;
    }
    public Product p;
    public IActionResult OnGet(int? id)
    {
        if (id == null) return NotFound();
        p = productDB.Get((int)id);
        if (p == null)
        {
            return NotFound();
        }
        return Page();
    }
}