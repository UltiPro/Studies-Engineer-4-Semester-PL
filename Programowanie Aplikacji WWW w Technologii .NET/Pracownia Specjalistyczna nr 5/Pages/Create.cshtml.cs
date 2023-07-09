#pragma warning disable

using Microsoft.AspNetCore.Mvc;
using Models.Product;
using Models.MyPageModel;

namespace Pracownia_Specjalistyczna_nr_5.Pages;
public class CreateModel : MyPageModel
{
    [BindProperty]
    public Product newProduct { get; set; }
    public void OnGet() { }
    public IActionResult OnPost()
    {
        if (!ModelState.IsValid) return Page();
        LoadDB();
        productDB.Create(newProduct);
        SaveDB();
        return RedirectToPage("List");
    }
}