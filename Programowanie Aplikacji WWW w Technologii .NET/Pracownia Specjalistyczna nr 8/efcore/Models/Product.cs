using System.ComponentModel.DataAnnotations;

namespace efcore.Models
{
    public class Product
    {
        [Key]
        public int id { get; set; }
        [Display(Name = "name")]
        [Required]
        public string name { get; set; }
        [Display(Name = "price")]
        [Required]
        public decimal price { get; set; }
        public IList<ProductCategory> ProductCategorys { get; set; }
    }
}
