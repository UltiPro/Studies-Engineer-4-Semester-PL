using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace efcore.Models;

[Table("ProductCategory")]
public class ProductCategory
{
    [Display(Name = "ID")]
    public int id { get; set; }
    public Product Products { get; set; }
    public int ProductId { get; set; }
    public Category Categories { get; set; }
    public int CategoryId { get; set; }
}