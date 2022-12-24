#pragma warning disable

using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Models.MyPageModel;
using Models.Product;
using Models.Category;
using DAL.ProductDB;
using DAL.CategoryDB;

namespace Pracownia_Specjalistyczna_nr_5.Pages;

public class EditModel : MyPageModel
{
    ProductDB pdb = new ProductDB();
    CategoryDB cdb = new CategoryDB();
    [FromQuery(Name = "id")]
    public int id { get; set; }
    [BindProperty]
    public Product p { get; set; }
    public List<Category> categoryList { get; set; }
    public IActionResult OnGet(int? id)
    {
        if(id == null) return NotFound();
        p = pdb.GetProductData(id);
        if(p == null) return NotFound();
        categoryList = cdb.GetAllCategories().ToList();
        return Page();
    }
    public IActionResult OnPost()
    {
        if (!ModelState.IsValid) return Page();
        p.id = (int)id;
        pdb.UpdateProduct(p);
        return RedirectToPage("List");
    }
}
