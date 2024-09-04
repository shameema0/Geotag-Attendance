using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;


namespace MajorProject
{
    public partial class Login : System.Web.UI.Page
    {
        string connectionString = @"Data Source=DESKTOP-FOA8QPN\SQLEXPRESS;Initial Catalog=NEWSHAMEE;User ID=shameema;Password=snazrin05";

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //string connectionString = @"Data Source=192.169.1.167\SQL2017;Initial Catalog=SHAMEEMA;User ID=sa;Password=welcome3#";
            SqlConnection con = new SqlConnection(connectionString);
            string usname = TextBox1.Text.ToString();
            if (usname == "Admin")
            {
                string q1 = "select Password from UserTab where Username ='Admin'";
                SqlCommand cmd = new SqlCommand(q1, con);
                SqlDataReader reader;
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string passw = (string)reader["Password"];
                    if (passw == TextBox2.Text)
                    {
                        Session["username"] = TextBox1.Text;
                        
                        Label3.Text = "Success";
                        //Response.Redirect("Admin_Home.aspx");
                        Response.Redirect("AdminHome.aspx");

                    }
                    else
                    {
                        Label3.Text = "Invalid Credentials";
                    }
                }

                con.Close();
            }
            else
            {
                //Employee Module - Employee Table username and Password
                //View Profile
                //Mark Attendance
                //View Attendance
                //Label4.Text = "Not Admin";
                string q2 = "select Password from Employee_Details where Username ='" + usname + "'";
                SqlCommand cmd = new SqlCommand(q2, con);
                SqlDataReader reader;
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string passw = (string)reader["Password"];
                    if (passw == TextBox2.Text)
                    {
                        Session["username"] = TextBox1.Text;
                       
                        Label3.Text = "Success";
                        Response.Redirect("Trial.aspx");

                    }
                    else
                    {
                        Label3.Text = "Invalid Credentials";
                    }
                }

                con.Close();
            }
        }
    }
}