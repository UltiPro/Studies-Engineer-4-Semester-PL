#pragma warning disable

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Models.MyPageModel;
using Models.Product;
using Models.Category;
using DAL.ProductDB;
using DAL.CategoryDB;

namespace Pracownia_Specjalistyczna_nr_5.Pages;

public class ListModel : MyPageModel
{
    ProductDB pdb = new ProductDB();
    CategoryDB cdb = new CategoryDB();
    public List<Product> productList { get; set; }
    public List<Category> categoryList { get; set; }
    public async void OnGet()
    {
        productList = pdb.GetAllProducts().ToList();
        categoryList = cdb.GetAllCategories().ToList();
    }
}