#pragma warning disable

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Models.Product;
using Models.MyPageModel;
using System.Text.RegularExpressions;

namespace Pracownia_Specjalistyczna_nr_5.Pages;

public class DeleteModel : MyPageModel
{
    public IActionResult OnGet(int? id)
    {
        LoadDB();
        productDB.Remove((int)id);
        SaveDB();
        var cookie = Request.Cookies["Shopping"];
        if (cookie != null) cookie = Regex.Replace(cookie,id.ToString(),String.Empty);
        Response.Cookies.Append("Shopping", cookie);
        return RedirectToPage("List");
    }
}
