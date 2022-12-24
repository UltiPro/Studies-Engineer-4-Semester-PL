#pragma warning disable

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Models.Person;

namespace Pracownia_Specjalistyczna_nr_4.Pages;

public class BmiModel : PageModel
{
    public Person p;
    public double bmi;
    public void OnGet(Person person)
    {
        p = person;
        if(p.heightInCm) bmi = Math.Round((((double)p.weight) / Math.Pow((double)p.height, 2))*10000,2);
        else bmi = Math.Round((((double)p.weight) / Math.Pow((double)p.height, 2)),2);
    }
}