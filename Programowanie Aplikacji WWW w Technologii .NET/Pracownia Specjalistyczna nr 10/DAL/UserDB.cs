#pragma warning disable

using System;
using System.Collections.Generic;
using System.Data;
using Models.User;
using System.Data.SqlClient;

namespace DAL.UserDB;

public class UserDB
{
    string connectionString = "data source=DESKTOP-3CDO0C0\\SQLEXPRESS;initial catalog=Users;trusted_connection=true";
    public User GetUser(string Login, string Password)
    {
        User u = new User();

        using (SqlConnection con = new SqlConnection(connectionString))
        {
            SqlCommand cmd = new SqlCommand("GetUser", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@login", Login);
            cmd.Parameters.AddWithValue("@password", Password);

            con.Open();
            SqlDataReader rdr = cmd.ExecuteReader();

            if (!(rdr.Read())) return null;

            u.id = Convert.ToInt32(rdr["UserId"]);
            u.login = rdr["UserName"].ToString();
            u.password = rdr["Password"].ToString();
            u.active = Convert.ToBoolean(rdr["IsActive"]);
        }

        return u;
    }
}