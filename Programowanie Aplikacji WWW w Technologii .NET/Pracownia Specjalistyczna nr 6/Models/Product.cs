#pragma warning disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models.Product;

public class Product
{
    [Required(ErrorMessage = "To pole jest wymagane!")]
    [Display(Name = "Id")]
    public int id { get; set; }
    [Required(ErrorMessage = "To pole jest wymagane!")]
    [Display(Name = "Nazwa")]
    public string name { get; set; }
    [Required(ErrorMessage = "To pole jest wymagane!")]
    [Range(0, Double.PositiveInfinity, ErrorMessage = "Zakres od 0 do nieskończoności!")]
    [Display(Name = "Cena")]
    public decimal? price { get; set; }
}

