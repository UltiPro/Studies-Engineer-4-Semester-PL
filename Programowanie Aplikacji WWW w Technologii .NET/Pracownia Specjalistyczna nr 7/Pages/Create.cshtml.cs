#pragma warning disable

using Microsoft.AspNetCore.Mvc;
using Models.MyPageModel;
using Models.Product;
using Models.Category;
using DAL.ProductDB;
using DAL.CategoryDB;

namespace Pracownia_Specjalistyczna_nr_5.Pages;
public class CreateModel : MyPageModel
{
    ProductDB pdb = new ProductDB();
    CategoryDB cdb = new CategoryDB();
    [BindProperty]
    public Product newProduct { get; set; }
    public List<Category> categoryList { get; set; }
    public void OnGet() 
    {
        categoryList = cdb.GetAllCategories().ToList();
    }
    public IActionResult OnPost()
    {
        if (!ModelState.IsValid) return Page();
        pdb.AddProduct(newProduct);
        return RedirectToPage("List");
    }
}
