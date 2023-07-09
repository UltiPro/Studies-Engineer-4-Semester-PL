#pragma warning disable

using Microsoft.AspNetCore.Mvc;
using Models.Product;
using Models.MyPageModel;
using DAL.ProductDB;

namespace Pracownia_Specjalistyczna_nr_5.Pages;
public class CreateModel : MyPageModel
{
    ProductDB pdb = new ProductDB();
    [BindProperty]
    public Product newProduct { get; set; }
    public void OnGet() { }
    public IActionResult OnPost()
    {
        if (!ModelState.IsValid) return Page();
        pdb.AddProduct(newProduct);
        return RedirectToPage("List");
    }
}