#pragma warning disable

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Xml;
using Models.Product;
using DAL.Product2;

namespace Pracownia_Specjalistyczna_nr_9.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    public List<Product> productList = new List<Product>();
    IProductDB productDB;
    public IndexModel(IProductDB _productDB)
    {
        productDB = _productDB;
    }
    public void OnGet()
    {
        productList = productDB.List();
    }
    private Product XmlNode2Product(XmlNode node)
    {
        Product p = new Product();
        p.id = int.Parse(node.Attributes["id"].Value);
        p.name = node["name"].InnerText;
        p.price = decimal.Parse(node["price"].InnerText);
        return p;
    }
}
