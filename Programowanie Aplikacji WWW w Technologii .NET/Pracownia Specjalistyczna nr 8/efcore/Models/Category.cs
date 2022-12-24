using System.ComponentModel.DataAnnotations;

namespace efcore.Models
{
    public class Category
    {
        public int ID { get; set; }
        [Display(Name = "category_name")]
        public string name { get; set; }
        [Display(Name = "short_cat_name")]
        public string s_n { get; set; }
        public IList<ProductCategory> ProductCategorys { get; set; }

    }
}
