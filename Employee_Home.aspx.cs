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
    public partial class Employee_Home : System.Web.UI.Page
    {
        string connectionString = @"Data Source=DESKTOP-FOA8QPN\SQLEXPRESS;Initial Catalog=NEWSHAMEE;User ID=shameema;Password=snazrin05";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DropDownList2.SelectedIndex = 1;
            }
        }
        public static double DistanceTo(double lat1, double lon1, double lat2, double lon2, char unit = 'K')
        {
            double rlat1 = Math.PI * lat1 / 180;
            double rlat2 = Math.PI * lat2 / 180;
            double theta = lon1 - lon2;
            double rtheta = Math.PI * theta / 180;
            double dist =
                Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) *
                Math.Cos(rlat2) * Math.Cos(rtheta);
            dist = Math.Acos(dist);
            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;
            dist = dist * 1.609344;
            return dist;

        }


        protected void PunchIn_Click(object sender, EventArgs e)
        {
            string curloc = LatLongitude.Value;
            string empid = DropDownList1.SelectedItem.Value.ToString();


            DateTime now = DateTime.Today;
            string nowdate = now.ToString("dd-MM-yyyy 00:00:00");

            SqlConnection con12 = new SqlConnection(connectionString);
            string q12 = "select In_Date,In_Time,Out_Time from Employee_Attendance where Emp_ID='" + empid + "'";

            SqlCommand cmd12 = new SqlCommand(q12, con12);
            SqlDataReader reader12;

            con12.Open();
            reader12 = cmd12.ExecuteReader();

            while (reader12.Read())
            {
                DateTime inDate = (DateTime)reader12["In_Date"];
                // object inTime = reader12["In_Time"];
                //object outTime = reader12["Out_Time"];

                TimeSpan induration = (TimeSpan)reader12["In_Time"];
                TimeSpan outduration = (TimeSpan)reader12["Out_Time"];

                DateTime referenceDate = DateTime.Today;

                DateTime inTime = referenceDate.Add(induration);
                DateTime outTime = referenceDate.Add(outduration);


                string indate = inDate.ToString();
                string intime = inTime.ToString("h:mm:ss tt");
                string outtime = outTime.ToString("h:mm:ss tt");

                if (indate == nowdate)
                {
                    Response.Write("<div class='alert alert-danger alert-dismissible fade show' style='text-align:center' role='alert'>" + "Cannot Punch-In! Attendance has already been recorded for Today!" + "<button type='button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button>" + "</div>");
                    TextBox2.Text = " ";
                    Label6.Text = "In Time: " + intime + "     Out Time: " + outtime;
                    Label4.Text = " ";
                    return;
                }
            }

            con12.Close();

            SqlConnection con14 = new SqlConnection(connectionString);
            string q14 = "select LatLng from BranchLoc where BranchName='" + TextBox1.Text + "'";
            SqlCommand cmd14 = new SqlCommand(q14, con14);
            SqlDataReader reader14;

            con14.Open();
            reader14 = cmd14.ExecuteReader();

            while (reader14.Read())
            {
                object latlngObject = reader14["LatLng"];

                if (latlngObject == DBNull.Value)
                {
                    Response.Write("<div class='alert alert-danger alert-dismissible fade show' style='text-align:center' role='alert'>" + "Pin Branch Location to enable attendance for employee!" + "<button type='button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button>" + "</div>");
                    TextBox2.Text = " ";
                    Label4.Text = " ";
                    return;
                }
            }
            con14.Close();


            SqlConnection con = new SqlConnection(connectionString);
            string q2 = "select LatLng,BranchID from BranchLoc where BranchName='" + TextBox1.Text + "'";
            SqlCommand cmd = new SqlCommand(q2, con);
            SqlDataReader reader;
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string mapaddress = (string)reader["LatLng"];
                int branchid = (int)reader["BranchID"];
                string[] curlocArray = curloc.Split(',');
                string[] mapaddressArray = mapaddress.Split(',');


                double curlocLat = Convert.ToDouble(curlocArray[0]);
                double curlocLon = Convert.ToDouble(curlocArray[1]);
                double mapaddressLat = Convert.ToDouble(mapaddressArray[0]);
                double mapaddressLon = Convert.ToDouble(mapaddressArray[1]);


                double dist = DistanceTo(curlocLat, curlocLon, mapaddressLat, mapaddressLon, 'K');
                //dist = Math.Round(dist);
                if (dist <= 1.0)
                {

                    DateTime noww = DateTime.Today;
                    string nowdate1 = noww.ToString("MM-dd-yyyy 00:00:00");
                    string nowtime = DateTime.Now.ToString("h:mm:ss tt");

                    empinaddress.Value = TextBox2.Text;
                    SqlConnection con1 = new SqlConnection(connectionString);
                    string q6 = "insert into Employee_Attendance (Emp_ID,BranchID,In_Date,In_Time,In_Address, In_LatLng) values ('" + empid + "','" + branchid + "','" + nowdate1 + "','" + nowtime + "','" + TextBox2.Text + "','" + curloc + "')";

                    SqlCommand cmd3 = new SqlCommand(q6, con1);
                    con1.Open();

                    cmd3.ExecuteNonQuery();
                    con1.Close();

                    Response.Write("<div class='alert alert-success alert-dismissible fade show' style='text-align:center' role='alert'>" + "Within 1 km radius! Punched In Successfully" + "<button type='button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button>" + "</div>");

                    PunchOut.Enabled = true;
                    PunchIn.Enabled = false;
                    TextBox2.Text = " ";
                    newlatilngi.Value = curloc;
                    empintime.Value = nowtime;

                    Label4.Text = "In Time: " + nowtime;
                    Label6.Text = " ";

                }

                else
                {

                    Response.Write("<div class='alert alert-danger alert-dismissible fade show' style='text-align:center' role='alert'>" + "Outside 1 km radius! Cannot Punch-In" + "<button type='button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button>" + "</div>");
                    TextBox2.Text = " ";

                }
            }
            con.Close();
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Label6.Text = " ";
            Label4.Text = " ";
            PunchIn.Enabled = true;
            PunchOut.Enabled = false;
            string empid = DropDownList1.SelectedItem.Value.ToString();
            if (empid == "0")
            {
                TextBox1.Text = " ";
                TextBox2.Text = " ";
            }
            else
            {

                SqlConnection con = new SqlConnection(connectionString);

                string q1 = "select BranchName from BranchLoc WHERE BranchID=(SELECT BranchID FROM Emp_Details where Emp_ID='" + empid + "')";
                SqlCommand cmd = new SqlCommand(q1, con);
                SqlDataReader reader;

                con.Open();
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string branchname = (string)reader["BranchName"];
                    TextBox1.Text = branchname;
                    newlatilngi.Value = " ";
                    outlatilngi.Value = " ";
                }

                reader.Close();
                con.Close();
            }
            TextBox2.Text = " ";


            DateTime now = DateTime.Today;
            string nowdate = now.ToString("dd-MM-yyyy 00:00:00");

            //empid with Indate exists enable punch out else no need
            SqlConnection con20 = new SqlConnection(connectionString);
            string q20 = "select In_Date, Out_Date,In_LatLng,Out_LatLng,In_Time,Out_Time,In_Address, Out_Address from Employee_Attendance where Emp_ID='" + empid + "'";
            SqlCommand cmd20 = new SqlCommand(q20, con20);
            SqlDataReader reader20;

            con20.Open();
            reader20 = cmd20.ExecuteReader();

            while (reader20.Read())
            {

                DateTime inDate = (DateTime)reader20["In_Date"];
                object outDate = reader20["Out_Date"];
                object latlng = reader20["In_LatLng"];
                object outlatlng = reader20["Out_LatLng"];
                TimeSpan induration = (TimeSpan)reader20["In_Time"];
                object outTime = reader20["Out_Time"];
                object inadd = reader20["In_Address"];
                object outadd = reader20["Out_Address"];

                DateTime referenceDate = DateTime.Today;
                DateTime inTime = referenceDate.Add(induration);

                string intime = inTime.ToString("h:mm:ss tt");
                string indate = inDate.ToString();
                string outdate = outDate.ToString();
                string latlong = latlng.ToString();
                string outlatlong = outlatlng.ToString();
                string inaddress = inadd.ToString();
                string outaddress = outadd.ToString();

                if (outTime == DBNull.Value)
                {
                    string outtime = outTime.ToString();
                    if ((indate == nowdate) && !(outdate == nowdate))
                    {
                        PunchOut.Enabled = true;
                        PunchIn.Enabled = false;
                        newlatilngi.Value = latlong;
                        outlatilngi.Value = outlatlong;
                        empintime.Value = intime;
                        empinaddress.Value = inaddress;
                        empoutaddress.Value = outaddress;
                        Label4.Text = "In Time: " + intime;
                        Label6.Text = " ";
                    }
                    else if ((indate != nowdate) && (outdate != nowdate))
                    {
                        PunchOut.Enabled = false;
                        PunchIn.Enabled = true;

                    }
                }
                else
                {
                    TimeSpan outduration = (TimeSpan)reader20["Out_Time"];
                    DateTime outTim = referenceDate.Add(outduration);

                    string outtime = outTim.ToString("h:mm:ss tt");
                    if ((indate == nowdate) && (outdate == nowdate))
                    {
                        PunchOut.Enabled = false;
                        PunchIn.Enabled = false;
                        newlatilngi.Value = latlong;
                        outlatilngi.Value = outlatlong;
                        empintime.Value = intime;
                        empouttime.Value = outtime;
                        empinaddress.Value = inaddress;
                        empoutaddress.Value = outaddress;
                        Label6.Text = "In Time: " + intime + "     Out Time: " + outtime;
                        Label4.Text = " ";
                        Response.Write("<div class='alert alert-danger alert-dismissible fade show' style='text-align:center' role='alert'>" + "Cannot Punch-In! Attendance has already been recorded for Today!" + "<button type='button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button>" + "</div>");


                    }
                    else if ((indate == nowdate) && !(outdate == nowdate))
                    {
                        PunchOut.Enabled = true;
                        PunchIn.Enabled = false;
                        newlatilngi.Value = latlong;
                        outlatilngi.Value = outlatlong;
                        empintime.Value = intime;
                        empinaddress.Value = inaddress;
                        empoutaddress.Value = outaddress;
                        Label4.Text = "In Time: " + intime;
                        Label6.Text = " ";
                    }
                    else if ((indate != nowdate) && (outdate != nowdate))
                    {
                        PunchOut.Enabled = false;
                        PunchIn.Enabled = true;

                    }
                }

            }

            reader20.Close();
            con20.Close();
        }

        protected void PunchOut_Click(object sender, EventArgs e)
        {
            PunchIn.Enabled = false;
            string empid = DropDownList1.SelectedItem.Value.ToString();
            string curloc = LatLongitude.Value;

            DateTime now = DateTime.Today;
            string nowdate = now.ToString("dd-MM-yyyy 00:00:00");

            SqlConnection con12 = new SqlConnection(connectionString);
            string q12 = "select Out_Date from Employee_Attendance where Emp_ID='" + empid + "'";

            SqlCommand cmd12 = new SqlCommand(q12, con12);
            SqlDataReader reader12;

            con12.Open();
            reader12 = cmd12.ExecuteReader();

            while (reader12.Read())
            {
                object outDate = reader12["Out_Date"];
                string outdate = outDate.ToString();

                if (outdate == nowdate)
                {
                    PunchIn.Enabled = true;
                    PunchOut.Enabled = false;
                    return;
                }
            }

            con12.Close();

            SqlConnection con = new SqlConnection(connectionString);
            string q2 = "select LatLng from BranchLoc where BranchName='" + TextBox1.Text + "'";
            SqlCommand cmd = new SqlCommand(q2, con);
            SqlDataReader reader;
            con.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string mapaddress = (string)reader["LatLng"];

                string[] curlocArray = curloc.Split(',');
                string[] mapaddressArray = mapaddress.Split(',');


                double curlocLat = Convert.ToDouble(curlocArray[0]);
                double curlocLon = Convert.ToDouble(curlocArray[1]);
                double mapaddressLat = Convert.ToDouble(mapaddressArray[0]);
                double mapaddressLon = Convert.ToDouble(mapaddressArray[1]);


                double dist = DistanceTo(curlocLat, curlocLon, mapaddressLat, mapaddressLon, 'K');
                //dist = Math.Round(dist);
                if (dist <= 1.0)
                {
                    string nowtime = DateTime.Now.ToString("h:mm:ss tt");

                    DateTime noww = DateTime.Today;
                    string nowdate1 = noww.ToString("MM-dd-yyyy 00:00:00");
                    empoutaddress.Value = TextBox2.Text;
                    SqlConnection con1 = new SqlConnection(connectionString);
                    string q4 = "update Employee_Attendance set Out_Date='" + nowdate1 + "',Out_Time='" + nowtime + "', Out_Address='" + TextBox2.Text + "', Out_LatLng='" + curloc + "' where Emp_ID='" + empid + "' AND In_Date='" + nowdate1 + "' AND Out_Date IS NULL ";
                    SqlCommand cmd1 = new SqlCommand(q4, con1);
                    con1.Open();
                    cmd1.ExecuteNonQuery();
                    con1.Close();

                    Response.Write("<div class='alert alert-success alert-dismissible fade show' style='text-align:center' role='alert'>" + "Within 1 km radius! Punched Out Successfully" + "<button type='button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button>" + "</div>");

                    SqlConnection con23 = new SqlConnection(connectionString);
                    string q23 = "select In_Time,In_Address from Employee_Attendance where Emp_ID='" + empid + "'";

                    SqlCommand cmd23 = new SqlCommand(q23, con23);
                    SqlDataReader reader23;

                    con23.Open();
                    reader23 = cmd23.ExecuteReader();

                    while (reader23.Read())
                    {
                        object inadd = reader23["In_Address"];
                        TimeSpan induration = (TimeSpan)reader23["In_Time"];

                        DateTime referenceDate = DateTime.Today;

                        DateTime inTime = referenceDate.Add(induration);

                        string intime = inTime.ToString("h:mm:ss tt");
                        string inaddress = inadd.ToString();

                        empintime.Value = intime;
                        empinaddress.Value = inaddress;

                        Label6.Text = "In Time: " + intime + " Out Time: " + nowtime;
                        Label4.Text = " ";

                    }

                    con12.Close();

                    PunchIn.Enabled = false;
                    PunchOut.Enabled = false;
                    TextBox2.Text = " ";
                    outlatilngi.Value = curloc;

                    empouttime.Value = nowtime;


                }

                else
                {

                    Response.Write("<div class='alert alert-danger alert-dismissible fade show' style='text-align:center' role='alert'>" + "Outside 1 km radius! Cannot Punch-Out" + "<button type='button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button>" + "</div>");
                    TextBox2.Text = " ";

                }
            }
            con.Close();
        }

        protected void DropDownList2_SelectedIndexChanged(object sender, EventArgs e)
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
    }
}

