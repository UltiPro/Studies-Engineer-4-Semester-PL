#pragma warning disable

using System.ComponentModel.DataAnnotations;

namespace Models.Product;

public class Product
{
    [Display(Name = "Id")]
    public int id { get; set; }

    [Display(Name = "Nazwa produktu")]
    public string name { get; set; }
    [Display(Name = "Cena")]
    public decimal price { get; set; }
}