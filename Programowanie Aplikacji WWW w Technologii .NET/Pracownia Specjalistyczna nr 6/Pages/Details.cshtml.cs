#pragma warning disable

using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Models.Product;
using Models.MyPageModel;
using DAL.ProductDB;

namespace Pracownia_Specjalistyczna_nr_5.Pages;

public class DetailsModel : MyPageModel
{
    ProductDB pdb = new ProductDB();
    public Product p;
    public IActionResult OnGet(int? id)
    {
        if (id == null) return NotFound();
        p = pdb.GetProductData(id);
        if (p == null)
        {
            return NotFound();
        }
        return Page();
    }
}