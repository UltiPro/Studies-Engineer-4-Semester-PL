#pragma warning disable

using System;
using System.Collections.Generic;
using System.Data;
using Models.Category;
using System.Data.SqlClient;

namespace DAL.CategoryDB;

public class CategoryDB
{
    string connectionString = "data source=DESKTOP-3CDO0C0\\SQLEXPRESS;initial catalog=MyCompany;trusted_connection=true";
    public IEnumerable<Category> GetAllCategories()
    {
        List<Category> listOfCategories = new List<Category>();

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            SqlCommand cmd = new SqlCommand("GetCategories", con);
            cmd.CommandType = CommandType.StoredProcedure;

            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                Category c = new Category();

                c.id = Convert.ToInt32(rdr["Id"]);
                c.shortName = rdr["shortName"].ToString();
                c.longName = rdr["longName"].ToString();

                listOfCategories.Add(c);
            }

            con.Close();
        }

        return listOfCategories;
    }
    public Category GetCategoryData(int? id)
    {
        Category c = new Category();

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            SqlCommand cmd = new SqlCommand("GetCategory", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@CId", id);

            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                c.id = Convert.ToInt32(rdr["Id"]);
                c.shortName = rdr["shortName"].ToString();
                c.longName = rdr["longName"].ToString();
            }
        }
        return c;
    }
}