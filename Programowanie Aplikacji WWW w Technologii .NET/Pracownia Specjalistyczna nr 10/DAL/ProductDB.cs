#pragma warning disable

using System;
using System.Collections.Generic;
using System.Data;
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
            SqlCommand cmd = new SqlCommand("GetProducts", con);  
            cmd.CommandType = CommandType.StoredProcedure;

            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                Product p = new Product();

                p.id = Convert.ToInt32(rdr["Id"]);
                p.name = rdr["name"].ToString();
                p.price = Convert.ToDecimal(rdr["price"]);
                p.id_cat = Convert.ToInt32(rdr["Id_cat"]);

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
            SqlCommand cmd = new SqlCommand("AddProduct", con);  
            cmd.CommandType = CommandType.StoredProcedure;  
  
            cmd.Parameters.AddWithValue("@Name", p.name);  
            cmd.Parameters.AddWithValue("@Money", p.price);
            cmd.Parameters.AddWithValue("@CId", p.id_cat); 

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
            SqlCommand cmd = new SqlCommand("GetProduct",con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@PId", id);

            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                p.id = Convert.ToInt32(rdr["Id"]);
                p.name = rdr["name"].ToString();
                p.price = Convert.ToDecimal(rdr["price"]);
                p.id_cat = Convert.ToInt32(rdr["Id_cat"]);
            }
        }
        return p;
    }
    public void DeleteProduct(int? id)
    {
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            SqlCommand cmd = new SqlCommand("DeleteProduct", con);  
            cmd.CommandType = CommandType.StoredProcedure;  
  
            cmd.Parameters.AddWithValue("@PId", id);  

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }
    }
    public void UpdateProduct(Product p)
    {
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            SqlCommand cmd = new SqlCommand("UpdateProduct", con);  
            cmd.CommandType = CommandType.StoredProcedure;  
  
            cmd.Parameters.AddWithValue("@PId", p.id);  
            cmd.Parameters.AddWithValue("@Name", p.name);  
            cmd.Parameters.AddWithValue("@Money", p.price);
            cmd.Parameters.AddWithValue("@CId", p.id_cat);   

            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }
    }
}
