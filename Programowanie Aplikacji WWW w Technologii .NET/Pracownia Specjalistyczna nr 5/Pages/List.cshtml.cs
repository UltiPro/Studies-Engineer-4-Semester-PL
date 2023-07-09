#pragma warning disable

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Models.Product;
using Models.MyPageModel;

namespace Pracownia_Specjalistyczna_nr_5.Pages;

public class ListModel : MyPageModel
{
    public List<Product> productList;
    public async void OnGet()
    {
        LoadDB();
        productList = productDB.List();
        SaveDB();
    }
}