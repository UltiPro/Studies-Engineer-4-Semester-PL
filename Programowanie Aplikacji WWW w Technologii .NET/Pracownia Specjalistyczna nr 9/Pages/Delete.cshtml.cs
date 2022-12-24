#pragma warning disable

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Xml;
using Models.Product;
using DAL.Product2;

namespace Pracownia_Specjalistyczna_nr_9.Pages;

public class DeleteModel : PageModel
{
    IProductDB productDB;
    public DeleteModel(IProductDB _productDB)
    {
        productDB = _productDB;
    }
    public IActionResult OnGet(int? id)
    {
        if (id == null) return NotFound();
        productDB.Delete((int)id);
        return RedirectToPage("Index");
    }
}
