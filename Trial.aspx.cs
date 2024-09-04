using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;

namespace MajorProject
{
    public partial class Trial : System.Web.UI.Page
    {
        string connectionString = @"Data Source=DESKTOP-FOA8QPN\SQLEXPRESS;Initial Catalog=NEWSHAMEE;User ID=shameema;Password=snazrin05";

        protected void Page_Load(object sender, EventArgs e)
        {
            string usname = Session["username"].ToString();
            SqlConnection con12 = new SqlConnection(connectionString);
            string q12 = "select Emp_ID,Name from Employee_Details where Username='" + usname + "'";

            SqlCommand cmd12 = new SqlCommand(q12, con12);
            SqlDataReader reader12;

            con12.Open();
            reader12 = cmd12.ExecuteReader();

            while (reader12.Read())
            {
                int empid = (int)reader12["Emp_ID"];
                Label8.Text = empid.ToString();
                string empname = (string)reader12["Name"];
                Label10.Text = empname;

                SqlConnection con31 = new SqlConnection(connectionString);
                string q31 = "select BranchName from BranchLoc WHERE BranchID=(SELECT BranchID FROM Employee_Details where Emp_ID='" + empid + "')";

                SqlCommand cmd31 = new SqlCommand(q31, con31);
                SqlDataReader reader31;
                con31.Open();
                reader31=cmd31.ExecuteReader();
                while(reader31.Read())
                {
                    string branchname = (string)reader31["BranchName"];
                    TextBox1.Text = branchname;
                }
                con31.Close();
            }
            con12.Close();
            
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
            //string empid = DropDownList1.SelectedItem.Value.ToString();
            string empid = "1001";

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
                    //TextBox2.Text = " ";
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
                    //TextBox2.Text = " ";
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

                    //empinaddress.Value = TextBox2.Text;
                    SqlConnection con1 = new SqlConnection(connectionString);
                    string q6 = "insert into Employee_Attendance (Emp_ID,BranchID,In_Date,In_Time,In_Address, In_LatLng) values ('" + empid + "','" + branchid + "','" + nowdate1 + "','" + nowtime + "','Location ','" + curloc + "')";

                    SqlCommand cmd3 = new SqlCommand(q6, con1);
                    con1.Open();

                    cmd3.ExecuteNonQuery();
                    con1.Close();

                    Response.Write("<div class='alert alert-success alert-dismissible fade show' style='text-align:center' role='alert'>" + "Within 1 km radius! Punched In Successfully" + "<button type='button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button>" + "</div>");

                    PunchOut.Enabled = true;
                    PunchIn.Enabled = false;
                    //TextBox2.Text = " ";
                    newlatilngi.Value = curloc;
                    empintime.Value = nowtime;

                    Label4.Text = "In Time: " + nowtime;
                    Label6.Text = " ";

                }

                else
                {

                    Response.Write("<div class='alert alert-danger alert-dismissible fade show' style='text-align:center' role='alert'>" + "Outside 1 km radius! Cannot Punch-In" + "<button type='button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button>" + "</div>");
                    //TextBox2.Text = " ";

                }
            }
            con.Close();
        }

        

        protected void PunchOut_Click(object sender, EventArgs e)
        {
            PunchIn.Enabled = false;
            // string empid = DropDownList1.SelectedItem.Value.ToString();
            string empid = "1001";
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
                    //empoutaddress.Value = TextBox2.Text;
                    SqlConnection con1 = new SqlConnection(connectionString);
                    string q4 = "update Employee_Attendance set Out_Date='" + nowdate1 + "',Out_Time='" + nowtime + "', Out_Address='Location', Out_LatLng='" + curloc + "' where Emp_ID='" + empid + "' AND In_Date='" + nowdate1 + "' AND Out_Date IS NULL ";
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
                    //TextBox2.Text = " ";
                    outlatilngi.Value = curloc;

                    empouttime.Value = nowtime;


                }

                else
                {

                    Response.Write("<div class='alert alert-danger alert-dismissible fade show' style='text-align:center' role='alert'>" + "Outside 1 km radius! Cannot Punch-Out" + "<button type='button' class='btn-close' data-bs-dismiss='alert' aria-label='Close'></button>" + "</div>");
                    //TextBox2.Text = " ";

                }
            }
            con.Close();
        }

        protected void DropDownList2_SelectedIndexChanged(object sender, EventArgs e)
        {
            int page = Convert.ToInt32(DropDownList2.SelectedItem.Value);
            if (page == 0)
            {
                Response.Redirect("Login.aspx");
            }
            else if (page == 1)
            {
                Response.Redirect("Trial.aspx");
            }

        }

        protected void Upload_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                try
                {
                    // Save the file to the "Uploads" folder within your project
                    string fileName = FileUpload1.FileName;
                    string filePath = Server.MapPath("~/Uploads/") + fileName;
                    FileUpload1.SaveAs(filePath);

                    // Display the uploaded image using a relative URL
                    Image1.ImageUrl = "~/Uploads/" + fileName;
                    Image1.Visible = true;

                    Label5.Text = "File Uploaded: " + fileName;
                    var directories = ImageMetadataReader.ReadMetadata(filePath);

                    // Extract GPS information
                    var dateDirectory = directories.OfType<ExifSubIfdDirectory>().FirstOrDefault();
                    var gpsDirectory = directories.OfType<GpsDirectory>().FirstOrDefault();

                    if (gpsDirectory != null)
                    {
                        double latitude = GetCoordinate(gpsDirectory, GpsDirectory.TagLatitude);
                        double longitude = GetCoordinate(gpsDirectory, GpsDirectory.TagLongitude);

                        // Now, you have latitude and longitude values
                        Latitude.Text = $"{latitude}, {longitude}";
                        LatLongitude.Value = Latitude.Text;
                    }
                    else
                    {
                        Latitude.Text = "GPS information not found in the image.";
                    }

                    if (dateDirectory != null)
                    {
                        DateTime dateTaken = dateDirectory.GetDateTime(ExifDirectoryBase.TagDateTimeOriginal);

                        // Display the date and time information
                        DateandTime.Text = $"Date Taken: {dateTaken}";
                    }
                    else
                    {
                        DateandTime.Text = "Date and time information not found in the image.";
                    }
                }
                catch (Exception ex)
                {
                    Label5.Text = $"Error: {ex.Message}";
                }

            }

            else
            {
                Label5.Text = "No File Uploaded.";
            }


        }
        private double GetCoordinate(GpsDirectory gpsDirectory, int tagType)
        {
            var coordinates = gpsDirectory.GetRationalArray(tagType);

            if (coordinates != null && coordinates.Length >= 3)
            {
                // Access coordinates[0], coordinates[1], coordinates[2], etc.
                double degrees = coordinates[0].ToDouble();
                double minutes = coordinates[1].ToDouble();
                double seconds = coordinates[2].ToDouble();

                return degrees + (minutes / 60) + (seconds / 3600);
            }
            else
            {
                // Handle the case where coordinates is null or has fewer than three elements.
                Label1.Text = "Coordinates not found in the image.";
            }



            return double.NaN; // Handle invalid data

        }
    }
}