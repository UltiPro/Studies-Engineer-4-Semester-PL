using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Ps2.Person;

namespace PS2___Zadanie.Pages;

public class IndexModel : PageModel
{
    public void OnGet() {}
    public IActionResult OnPost(Person person)
    {
        return RedirectToPage("Welcome",person);
    }
}
