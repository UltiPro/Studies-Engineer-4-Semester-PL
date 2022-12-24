#pragma warning disable

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Xml;
using Models.Product;
using DAL.Product2;

namespace Pracownia_Specjalistyczna_nr_9.Pages;
public class CreateModel : PageModel
{
    IProductDB productDB;
    [BindProperty]
    public Product newProduct { get; set; }
    public CreateModel(IProductDB _productDB)
    {
        productDB = _productDB;
    }
    public IActionResult OnPost()
    {
        if (!ModelState.IsValid) return Page();
        productDB.Add(newProduct);
        return RedirectToPage("Index");
    }
}
