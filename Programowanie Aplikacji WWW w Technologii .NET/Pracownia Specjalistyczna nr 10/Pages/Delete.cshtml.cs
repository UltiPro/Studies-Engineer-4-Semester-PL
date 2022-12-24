#pragma warning disable

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Models.Product;
using Models.MyPageModel;
using System.Text.RegularExpressions;
using DAL.ProductDB;

namespace Pracownia_Specjalistyczna_nr_5.Pages;

public class DeleteModel : MyPageModel
{
    ProductDB pdb = new ProductDB();
    public Product p { get; set; }
    byte[] sid;
    public IActionResult OnGet(int? id)
    {
        if(!HttpContext.Session.TryGetValue("id",out sid)) return RedirectToPage("/Index");
        if (id == null) return NotFound();
        p = pdb.GetProductData(id);
        if (p == null) return NotFound();
        pdb.DeleteProduct(id);
        return RedirectToPage("List");
    }
}
