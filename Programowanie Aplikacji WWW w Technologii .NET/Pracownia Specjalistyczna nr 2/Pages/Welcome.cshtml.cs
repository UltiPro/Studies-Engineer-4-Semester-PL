using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Ps2.Person;

#pragma warning disable 8618

namespace PS2___Zadanie.Pages
{
    public class WelcomeModel : PageModel
    {
        public string imie { get; set; }
        public string nazwisko { get; set; }
        public int wiek { get; set; }
        public string plec { get; set; }
        public string telefon { get; set; }
        public bool cpp { get; set; }
        public bool chasztag { get; set; }
        public bool java { get; set; }
        public void OnGet(Person person)
        {
            imie = person.imie;
            nazwisko = person.nazwisko;
            wiek = person.wiek;
            plec = person.plec;
            telefon = person.telefon;
            cpp = person.cpp;
            chasztag = person.chasztag;
            java = person.java;
        }
    }
}