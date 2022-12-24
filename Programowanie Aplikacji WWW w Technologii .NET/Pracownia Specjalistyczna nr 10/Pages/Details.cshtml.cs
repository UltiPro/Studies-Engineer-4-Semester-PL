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

public class DetailsModel : MyPageModel
{
    ProductDB pdb = new ProductDB();
    CategoryDB cdb = new CategoryDB();
    public Product p;
    public Category c;
    byte[] sid;
    public IActionResult OnGet(int? id)
    {
        if(!HttpContext.Session.TryGetValue("id",out sid)) return RedirectToPage("/Index");
        if (id == null) return NotFound();
        p = pdb.GetProductData(id);
        if(p==null)
        {
            return NotFound();
        }
        c = cdb.GetCategoryData(p.id_cat);
        return Page();
    }
}
