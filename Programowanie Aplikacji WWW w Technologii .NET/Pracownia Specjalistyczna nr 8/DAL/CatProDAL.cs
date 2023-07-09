using System;
using efcore.Models;
using System.Data.SqlClient;

namespace efcore.DAL
{
    public class CatProDAL
    {
        string connectionString = "data source=DESKTOP-3CDO0C0\\SQLEXPRESS;initial catalog=Shop;trusted_connection=true";
        public string Product(int id)
        {
            string name = "";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Product WHERE id =" + id.ToString() + "", con);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                rdr.Read();
                name = rdr["name"].ToString();
                con.Close();
            }
            return name;
        }
        public string Category(int id)
        {
            string name = "";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Category WHERE id =" + id.ToString() + "", con);
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                rdr.Read();
                name = rdr["name"].ToString();
                con.Close();
            }
            return name;
        }
    }
}