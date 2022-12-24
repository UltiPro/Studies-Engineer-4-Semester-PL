﻿#pragma warning disable

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Models.Person;

namespace Pracownia_Specjalistyczna_nr_4.Pages;

public class KalkulatorModel : PageModel
{
    [BindProperty]
    public Person person { get; set; }
    public IActionResult OnPost()
    {
        if (!ModelState.IsValid) return Page();
        return RedirectToPage("Bmi", person);
    }
    public void OnGet() { }
}
