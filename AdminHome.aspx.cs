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
    public partial class AdminHome : System.Web.UI.Page
    {
        string connectionString = @"Data Source=DESKTOP-FOA8QPN\SQLEXPRESS;Initial Catalog=NEWSHAMEE;User ID=shameema;Password=snazrin05";

        protected void Page_Load(object sender, EventArgs e)
        {
            string latLongitude = LatLongitude.Value;
            newlatilngi.Value = latLongitude;

            //latilngi.Value = latLongitude;
            if (!IsPostBack)
            {
                DropDownList2.SelectedIndex = 0;


            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string address = TextBox1.Text.Trim().ToString();
            address = new string(address.Where(c => !char.IsPunctuation(c)).ToArray());
            string latLongitude = LatLongitude.Value;
            string postalCode = PostalCode.Value;
            string areaName = areaname.Value;

            //Label4.Text = LatLongitude.Value + " " + PostalCode.Value+" "+areaname.Value+" "+address;
            if (postalCode == "0" || postalCode == null || postalCode == "")
            {
                Response.Write("<div class='alert alert-danger alert-dismissible fade show'  style='text-align:center' role='alert'>" + "Pincode not found !Choose another location!" + "<button type='button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button>" + "</div>");
                SqlConnection con3 = new SqlConnection(connectionString);

                string q12 = "select MapAddress,LatLng from BranchLoc where BranchName='" + DropDownList1.SelectedItem.Value + "'";
                SqlCommand cmd = new SqlCommand(q12, con3);
                SqlDataReader reader2;

                con3.Open();
                reader2 = cmd.ExecuteReader();

                while (reader2.Read())
                {
                    object latlngObject = reader2["LatLng"];
                    object MapAddressObject = reader2["MapAddress"];
                    if (latlngObject != DBNull.Value)
                    {
                        string latlng = (string)latlngObject;
                        newlatilngi.Value = latlng;
                    }
                    else
                    {

                        newlatilngi.Value = null;
                    }
                    if (MapAddressObject != DBNull.Value)
                    {
                        string mapaddress = (string)MapAddressObject;
                        TextBox1.Text = mapaddress;
                    }
                    else
                    {

                        TextBox1.Text = " ";
                    }

                }
                con3.Close();
                //TextBox1.Text = " ";
            }
            else
            {

                int pincode2 = Convert.ToInt32(postalCode);


                SqlConnection conn = new SqlConnection(connectionString);
                string q2 = "select Pincode, MapAddress,LatLng from BranchLoc where BranchName='" + DropDownList1.SelectedItem.Value + "'";
                SqlCommand cmmd = new SqlCommand(q2, conn);
                SqlDataReader reader;
                conn.Open();
                reader = cmmd.ExecuteReader();
                while (reader.Read())
                {
                    string pincode = (string)reader["Pincode"];
                    object mapaddr = reader["MapAddress"];
                    object latln = reader["LatLng"];

                    if (latln != DBNull.Value)
                    {
                        string latlng = (string)latln;
                        newlatilngi.Value = latlng;
                    }
                    else
                    {

                        newlatilngi.Value = null;
                    }
                    if (mapaddr != DBNull.Value)
                    {
                        string mapadd = (string)mapaddr;

                        if (TextBox1.Text == mapadd)
                        {
                            Response.Write("<div class='alert alert-success alert-dismissible fade show' style='text-align:center' role='alert'>" + "Branch Location already pinned!" + "<button type='button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button>" + "</div>");

                            return;
                        }
                    }


                    int pincode1 = Convert.ToInt32(pincode);


                    int p2 = pincode1 + 10;

                    if (((areaName == DropDownList1.SelectedItem.Value) && !(pincode2 >= pincode1 && pincode2 <= p2)) || ((areaName == DropDownList1.SelectedItem.Value) && (pincode2 >= pincode1 && pincode2 <= p2)) || ((areaName != DropDownList1.SelectedItem.Value) && (pincode2 >= pincode1 && pincode2 <= p2)))

                    {
                        SqlConnection con = new SqlConnection(connectionString);

                        string query1 = "update BranchLoc set MapAddress='" + address + "', LatLng='" + latLongitude + "', SelectedPincode='" + pincode2 + "' where BranchName='" + DropDownList1.SelectedItem.Value + "'";


                        SqlCommand cmd = new SqlCommand(query1, con);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        newlatilngi.Value = latLongitude;
                        Response.Write("<div class='alert alert-success alert-dismissible fade show' style='text-align:center' role='alert'>" + "Branch Location updated successfully" + "<button type='button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button>" + "</div>");
                        //TextBox1.Text = " ";
                    }

                    else
                    {

                        Response.Write("<div class='alert alert-danger alert-dismissible fade show' style='text-align:center' role='alert'>" + "Map is pinned at irrelevant location! Choose another location!" + "<button type='button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button>" + "</div>");
                        SqlConnection con3 = new SqlConnection(connectionString);

                        string q12 = "select LatLng,MapAddress from BranchLoc where BranchName='" + DropDownList1.SelectedItem.Value + "'";
                        SqlCommand cmd = new SqlCommand(q12, con3);
                        SqlDataReader reader2;

                        con3.Open();
                        reader2 = cmd.ExecuteReader();

                        while (reader2.Read())
                        {
                            object latlngObject = reader2["LatLng"];
                            object MapAddressObject = reader2["MapAddress"];

                            if (latlngObject != DBNull.Value)
                            {
                                string latlng = (string)latlngObject;
                                newlatilngi.Value = latlng;
                            }
                            else
                            {

                                newlatilngi.Value = null;
                            }
                            if (MapAddressObject != DBNull.Value)
                            {
                                string mapaddress = (string)MapAddressObject;
                                TextBox1.Text = mapaddress;
                            }
                            else
                            {

                                TextBox1.Text = " ";
                            }

                        }
                        con3.Close();
                        //latilngi.Value = latLongitude;

                        //TextBox1.Text = " ";
                    }

                }

                conn.Close();
            }

        }
        protected void DropDownList2_SelectedIndexChanged1(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(DropDownList2.SelectedItem.Value);

            if (page == 1)
            {
                Response.Redirect("AdminHome.aspx");
            }
            else if (page == 2)
            {
                Response.Redirect("Employee_Home.aspx");
            }
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedlocdd = DropDownList1.SelectedItem.Text;
            if (selectedlocdd == "Select")
            {
                TextBox1.Text = " ";
            }
            else
            {
                SqlConnection con = new SqlConnection(connectionString);

                string q1 = "select MapAddress,LatLng,SelectedPincode from BranchLoc where BranchName='" + selectedlocdd + "'";
                SqlCommand cmd = new SqlCommand(q1, con);
                SqlDataReader reader;

                con.Open();
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    object latlngObject = reader["LatLng"];
                    object MapAddressObject = reader["MapAddress"];
                    object selPincode = reader["SelectedPincode"];
                    if (latlngObject != DBNull.Value)
                    {
                        string latlng = (string)latlngObject;
                        newlatilngi.Value = latlng;
                    }
                    else
                    {

                        newlatilngi.Value = null;
                    }
                    if (MapAddressObject != DBNull.Value)
                    {
                        string mapaddress = (string)MapAddressObject;
                        TextBox1.Text = mapaddress;
                    }
                    else
                    {

                        TextBox1.Text = " ";
                    }
                    if (selPincode != DBNull.Value)
                    {
                        string selectedpin = (string)selPincode;
                        PostalCode.Value = selectedpin;
                    }
                    else
                    {

                        PostalCode.Value = null;
                    }

                }
                con.Close();
                //TextBox1.Text = " ";
            }

        }


    }
}
