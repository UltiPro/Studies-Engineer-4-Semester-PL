#pragma warning disable

using Microsoft.AspNetCore.Mvc.RazorPages;
using DAL.ProductDB;
using Microsoft.AspNetCore.Mvc;
using Models.Product;
using System.Data;
using System.Data.SqlClient;
using Models.MyPageModel;

namespace Pracownia_Specjalistyczna_nr_5.Pages;

public class AddModel : MyPageModel
{
    public IConfiguration _configuration { get; }
    public string lblInfoText;
    public AddModel(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public void OnGet()
    {
        string connectionString = "data source=DESKTOP-3CDO0C0\\SQLEXPRESS;initial catalog=MyCompany;trusted_connection=true";
        SqlConnection con = new SqlConnection(connectionString);
        SqlCommand cmd = new SqlCommand("sp_productAdd", con);
        cmd.CommandType = CommandType.StoredProcedure;
        SqlParameter name_SqlParam = new SqlParameter("@name", SqlDbType.VarChar,
       50);
        name_SqlParam.Value = "baton";
        cmd.Parameters.Add(name_SqlParam);
        SqlParameter price_SqlParam = new SqlParameter("@price", SqlDbType.Money);
        price_SqlParam.Value = 4.50;
        cmd.Parameters.Add(price_SqlParam);
        SqlParameter productID_SqlParam = new SqlParameter("@productID",
       SqlDbType.Int);
        productID_SqlParam.Direction = ParameterDirection.Output;
        cmd.Parameters.Add(productID_SqlParam);
        con.Open();
        int numAff = cmd.ExecuteNonQuery();
        con.Close();
        lblInfoText += String.Format("Inserted <b>{0}</b> record(s)<br />", numAff);
        int productID = (int)cmd.Parameters["@productID"].Value;
        lblInfoText += "New ID: " + productID.ToString();
    }
}