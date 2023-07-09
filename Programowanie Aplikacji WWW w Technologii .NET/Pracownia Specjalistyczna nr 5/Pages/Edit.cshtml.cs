#pragma warning disable

using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Models.Product;
using Models.MyPageModel;

namespace Pracownia_Specjalistyczna_nr_5.Pages;

public class EditModel : MyPageModel
{

    [FromQuery(Name = "id")]
    public int id { get; set; }
    [BindProperty]
    public Product p { get; set; }
    public IActionResult OnPost()
    {
        if (!ModelState.IsValid) return Page();
        if (id == null) return NotFound();
        LoadDB();
        p.id = (int)id;
        productDB.UpDate(p);
        SaveDB();
        return RedirectToPage("List");
    }
}