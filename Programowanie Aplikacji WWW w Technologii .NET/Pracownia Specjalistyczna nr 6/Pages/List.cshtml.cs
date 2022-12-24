#pragma warning disable

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Models.Product;
using Models.MyPageModel;
using DAL.ProductDB;

namespace Pracownia_Specjalistyczna_nr_5.Pages;

public class ListModel : MyPageModel
{
    ProductDB pdb = new ProductDB();
    public List<Product> productList { get; set; }
    public async void OnGet()
    {
        productList = pdb.GetAllProducts().ToList();
    }
}
