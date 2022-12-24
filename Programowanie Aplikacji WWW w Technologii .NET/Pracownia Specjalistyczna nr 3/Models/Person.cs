#pragma warning disable

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Models.Person
{
    public class Person
    {
        [Required(ErrorMessage = "Pole Imie jest wymagane!")]
        [StringLength(15, ErrorMessage = "Imie nie może być dłuższe niż 15 znaków i krótsze niż 3 znaki!", MinimumLength = 3)]
        [RegularExpression(@"[a-zA-Z]+$", ErrorMessage = "Pole Imie może składać się tylko z liter!")]
        [Display(Name = "name")]
        public string name { get; set; }

        [EmailAddress(ErrorMessage = "Błędy adres e-mail!")]
        [Display(Name = "email")]
        public string? email { get; set; }

        [Required(ErrorMessage = "Pole Waga jest wymagane!")]
        [RegularExpression(@"(\d*\,)?\d+",ErrorMessage = "Błędna Waga!")]
        [Display(Name = "weight")]
        public double weight { get; set; }

        [Required(ErrorMessage = "Pole Wzrost jest wymagane!")]
        [RegularExpression(@"(\d*\,)?\d+",ErrorMessage = "Błędny wzrost!")]
        [Display(Name = "height")]
        public double height { get; set; }

        [Range(1, 120, ErrorMessage = "Zakres wieku jest od 1 do 120 lat!")]
        [RegularExpression(@"(^[1-9]{1}$)|(^[1-9]{1}[0-9]{1}$)|(^[1]{1}[0-1]{1}[0-9]{1}$)|(^[1]{1}[2]{1}[0]{1}$)", ErrorMessage = "Błędny wiek!")]
        [Display(Name = "age")]
        public int? age { get; set; }

        [Display(Name = "sex")]
        public string sex { get; set; }

        [Display(Name = "decision")]
        public bool heightInCm { get; set; }
    }
}