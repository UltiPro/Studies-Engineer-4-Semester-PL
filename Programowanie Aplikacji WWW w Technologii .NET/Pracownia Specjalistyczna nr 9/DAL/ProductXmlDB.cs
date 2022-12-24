#pragma warning disable

using System.Xml;
using Models.Product;
using DAL.Product2;

namespace WebApplication1.DAL
{
    public class ProductXmlDB : IProductDB
    {
        XmlDocument db = new XmlDocument();
        string xmlDB_path;
        public ProductXmlDB(IConfiguration _configuration)
        {
            xmlDB_path = _configuration.GetValue<string>("AppSettings:XmlDB_path");
            LoadDB();
        }
        private void LoadDB()
        {
            db.Load(xmlDB_path);
        }
        private void SaveDB()
        {
            db.Save(xmlDB_path);
        }
        public List<Product> List()
        {
            LoadDB();
            List<Product> productList = new List<Product>();
            XmlNodeList productXmlNodeList = db.SelectNodes("/store/product");
            foreach (XmlNode productXmlNode in productXmlNodeList)
            {
                productList.Add(XmlNodeProduct2Product(productXmlNode));
            }
            return productList;
        }
        private Product XmlNodeProduct2Product(XmlNode node)
        {
            LoadDB();
            Product p = new Product();
            p.id = int.Parse(node.Attributes["id"].Value);
            p.name = node["name"].InnerText;
            p.price = decimal.Parse(node["price"].InnerText);
            return p;
        }
        public Product Get(int _id)
        {
            LoadDB();
            Product p = new Product();
            List<Product> productList = List();
            p = productList.First(x => x.id == _id);
            if (p != null) return p;
            return null;
        }
        public int Update(Product _product)
        {
            Delete(_product.id);
            Add(_product);
            return 0;
        }
        public int Delete(int _id)
        {
            LoadDB();
            XmlNodeList productXmlNodeList = db.SelectNodes("/store/product");
            XmlNode ToRemove;
            foreach (XmlNode productXmlNode in productXmlNodeList)
            {
                if (int.Parse(productXmlNode.Attributes["id"].Value) == _id)
                {
                    productXmlNode.ParentNode.RemoveChild(productXmlNode);
                    break;
                }
            }
            SaveDB();
            return 0;
        }
        public int Add(Product _product)
        {
            LoadDB();
            XmlElement newProduct = db.CreateElement("product");
            newProduct.SetAttribute("id", (List().Count + 1).ToString());
            XmlElement name = db.CreateElement("name");
            name.InnerText = _product.name;
            XmlElement price = db.CreateElement("price");
            price.InnerText = _product.price.ToString();
            newProduct.AppendChild(name);
            newProduct.AppendChild(price);
            db.DocumentElement.AppendChild(newProduct.Clone());
            SaveDB();
            return 0;
        }
    }
}