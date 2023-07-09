#pragma warning disable

using System;
using System.Collections.Generic;
using Models.Product;
using System.Data.SqlClient;

namespace DAL.ProductDB;

public class ProductDB
{
    string connectionString = "data source=DESKTOP-3CDO0C0\\SQLEXPRESS;initial catalog=MyCompany;trusted_connection=true";
    public IEnumerable<Product> GetAllProducts()
    {
        List<Product> listOfProducts = new List<Product>();

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Product", con);

            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                Product p = new Product();

                p.id = Convert.ToInt32(rdr["Id"]);
                p.name = rdr["name"].ToString();
                p.price = Convert.ToDecimal(rdr["price"]);

                listOfProducts.Add(p);
            }

            con.Close();
        }

        return listOfProducts;
    }
    public void AddProduct(Product p)
    {
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            string sqlComand = $"INSERT INTO Product(name,price) VALUES('" + p.name + "','" + p.price + "')";
            SqlCommand cmd = new SqlCommand(sqlComand, con);

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }
    }
    public Product GetProductData(int? id)
    {
        Product p = new Product();

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            string sqlQuery = "SELECT * FROM Product WHERE id=" + id;
            SqlCommand cmd = new SqlCommand(sqlQuery, con);

            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                p.id = Convert.ToInt32(rdr["Id"]);
                p.name = rdr["name"].ToString();
                p.price = Convert.ToDecimal(rdr["price"]);
            }
        }
        return p;
    }
    public void DeleteProduct(int? id)
    {
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            SqlCommand cmd = new SqlCommand("DELETE FROM Product WHERE Id=" + id, con);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }
    }
    public void UpdateProduct(Product p)
    {
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            string SqlCommand = "UPDATE Product SET name='" + p.name + "', price=" + (int)p.price + " WHERE id=" + p.id;
            SqlCommand cmd = new SqlCommand(SqlCommand, con);

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }
    }
}