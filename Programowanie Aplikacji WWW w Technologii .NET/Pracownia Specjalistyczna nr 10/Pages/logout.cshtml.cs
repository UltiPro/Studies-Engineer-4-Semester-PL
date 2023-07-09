using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Pracownia_Specjalistyczna_nr_5.Pages;

public class LogoutModel : PageModel
{
    public IActionResult OnGet()
    {
        HttpContext.Session.Remove("id");
        HttpContext.Session.Remove("login");
        return RedirectToPage("Index");
    }
}