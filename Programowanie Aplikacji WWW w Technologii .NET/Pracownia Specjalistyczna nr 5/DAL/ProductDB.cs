#pragma warning disable

using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Models.Product;

namespace DAL.ProductDB;

public class ProductDB
{
    private List<Product> products;
    public void Load(string jsonProducts)
    {
        if (jsonProducts == null)
        {
            products = Product.GetProducts();
        }
        else
        {
            products = JsonSerializer.Deserialize<List<Product>>(jsonProducts);
        }
    }
    private int GetNextId()
    {
        int newID;
        if (products.Count != 0) newID = (products[products.Count - 1].id) + 1;
        else newID = 1;
        return newID;
    }
    public void Create(Product p)
    {
        p.id = GetNextId();
        products.Add(p);
    }
    public void UpDate(Product p)
    {
        var p2 = products.Find(m => m.id == p.id);
        products[products.IndexOf(p2)] = p;
    }
    public void Remove(int id)
    {
        products.Remove(products.First(m => m.id == id));
    }
    public string Save()
    {
        return JsonSerializer.Serialize(products);
    }
    public List<Product> List()
    {
        return products;
    }
}
