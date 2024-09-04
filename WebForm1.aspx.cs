using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web.UI.WebControls;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;

namespace MajorProject
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void UpdateAddress(object sender, EventArgs e)
        {
            // This method will be called when the "Understood" button is clicked.
            // You can write the code here to handle the button click event and update the address.

            // Example: Update the TextBox1.Text with a new value
            TextBox1.Text = "New Address Updated";

            // Add your logic to update the address or perform other actions as needed.
            // ...

            // Optionally, close the modal after handling the click event.
            //staticBackdrop.Hide();
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                try
                {
                    // Save the uploaded file
                    string imagePath = Server.MapPath("~/Uploads/") + FileUpload1.FileName;
                    FileUpload1.SaveAs(imagePath);

                    // Read metadata from the image
                    var directories = ImageMetadataReader.ReadMetadata(imagePath);

                    // Extract GPS information
                    var dateDirectory = directories.OfType<ExifSubIfdDirectory>().FirstOrDefault();
                    var gpsDirectory = directories.OfType<GpsDirectory>().FirstOrDefault();

                    if (gpsDirectory != null)
                    {
                        double latitude = GetCoordinate(gpsDirectory, GpsDirectory.TagLatitude);
                        double longitude = GetCoordinate(gpsDirectory, GpsDirectory.TagLongitude);

                        // Now, you have latitude and longitude values
                        Label2.Text = $"Latitude: {latitude}";
                        Label3.Text = $"Longitude: {longitude}";
                    }
                    else
                    {
                        Label1.Text = "GPS information not found in the image.";
                    }

                    if (dateDirectory != null)
                    {
                        DateTime dateTaken = dateDirectory.GetDateTime(ExifDirectoryBase.TagDateTimeOriginal);

                        // Display the date and time information
                        Label4.Text = $"Date Taken: {dateTaken}";
                    }
                    else
                    {
                        Label1.Text = "Date and time information not found in the image.";
                    }
                }
                catch (Exception ex)
                {
                    Label1.Text = $"Error: {ex.Message}";
                }
            }
            else
            {
                Label1.Text = "Please select an image to upload.";
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