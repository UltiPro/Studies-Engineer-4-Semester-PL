#pragma warning disable

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Session;
using System.Data.SqlClient;
using System.Text;
using Models.User;
using DAL.UserDB;

namespace Pracownia_Specjalistyczna_nr_5.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    public IConfiguration _configuration { get; }

    public IndexModel(IConfiguration configuration, ILogger<IndexModel> logger)
    {
        _logger = logger;
        _configuration = configuration;
    }

    [BindProperty]
    public string Username { get; set; }

    [BindProperty]
    public string Password { get; set; }

    public string Msg { get; set; }
    byte[] sid;

    public IActionResult OnGet()
    {
        if(HttpContext.Session.TryGetValue("id",out sid)) return RedirectToPage("/List");
        return Page();
    }

    public IActionResult OnPost()
    {
        UserDB con = new UserDB();
        
        User u;

        u = con.GetUser(Username,Password);

        if (u != null)
        {
            HttpContext.Session.SetInt32("id",u.id);
            HttpContext.Session.SetString("login", u.login);
            return RedirectToPage("List");
        }
        else
        {
            Msg = "Invalid";
            return Page();
        }
    }
}

