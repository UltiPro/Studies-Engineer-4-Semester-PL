using System;
using efcore.Models;
using System.Data.SqlClient;

namespace efcore.DAL
{
    public class CatDAL
    {
        string connectionString = "data source=DESKTOP-3CDO0C0\\SQLEXPRESS;initial catalog=Shop;trusted_connection=true";
        public List<Product> List(int id)
        {
            List<Product> listOfProducts = new List<Product>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Product p, ProductCategory pc WHERE p.id = pc.ProductId AND pc.CategoryId = " + id.ToString() + "", con);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Product p = new Product();
                    p.id = Convert.ToInt32(rdr["id"]);
                    p.name = rdr["name"].ToString();
                    p.price = Convert.ToDecimal(rdr["price"]);
                    listOfProducts.Add(p);
                }
                con.Close();
            }
            return listOfProducts;
        }
    }
}