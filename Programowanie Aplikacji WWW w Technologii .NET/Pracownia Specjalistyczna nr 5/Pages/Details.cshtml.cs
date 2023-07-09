#pragma warning disable

using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Models.Product;
using Models.MyPageModel;

namespace Pracownia_Specjalistyczna_nr_5.Pages;

public class DetailsModel : MyPageModel
{
    public Product p;
    public IActionResult OnGet(int? id)
    {
        if (id == null) return NotFound();
        LoadDB();
        p = productDB.List().FirstOrDefault(m => m.id == id);
        SaveDB();
        return Page();
    }
}