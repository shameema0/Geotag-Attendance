<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Employee_Home.aspx.cs" Inherits="MajorProject.Employee_Home" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <meta charset="UTF-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
   
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.1.3/dist/css/bootstrap.min.css"
        rel="stylesheet"
        integrity="sha384-1BmE4kWBq78iYhFldvKuhfTAU6auU8tT94WrHftjDbrCEXSU1oBoqyl2QvZ6jIW3"
        crossorigin="anonymous"
    />

 <script src="https://maps.googleapis.com/maps/api/js?key=AIzaSyCCgwzm9uzuvijnFWprbSJb-rJN-9GbhHU&callback=initMap" async defer></script>
    <script>
        var map;
        var marker;

        function initMap() {
            var initialLocation = { lat: 13.007780496481956, lng: 80.23899847554192 };

            map1 = new google.maps.Map(document.getElementById('preview'), {
                center: initialLocation,
                zoom: 13
            });


            var branchNameTextbox = document.getElementById('TextBox1');
            var selectedBranch = branchNameTextbox.value;
            var latLngString = document.getElementById('<%= newlatilngi.ClientID %>').value;
            var latLngArray = latLngString.split(',');
            var newLat = parseFloat(latLngArray[0]);
            var newLng = parseFloat(latLngArray[1]);

            var outlatLngString = document.getElementById('<%= outlatilngi.ClientID %>').value;
            var outlatLngArray = outlatLngString.split(',');
            var outLat = parseFloat(outlatLngArray[0]);
            var outLng = parseFloat(outlatLngArray[1]);

            var geocoder = new google.maps.Geocoder();

            geocoder.geocode({ 'address': selectedBranch }, function (results, status) {
                if (status == google.maps.GeocoderStatus.OK && results.length > 0) {
                    var location = results[0].geometry.location;
                    var lat = location.lat();
                    var lng = location.lng();

                    var initialLocation = { lat: lat, lng: lng };

                    map = new google.maps.Map(document.getElementById('preview'), {
                        center: initialLocation,
                        zoom: 13
                    });

                    marker = new google.maps.Marker({

                        map: map,
                        draggable: true
                    });
                    marker2 = new google.maps.Marker({

                        map: map,
                        draggable: true
                    });

                    var inTime = document.getElementById('<%= empintime.ClientID %>').value;
                   var outTime = document.getElementById('<%= empouttime.ClientID %>').value;
                   var inAddress = document.getElementById('<%= empinaddress.ClientID %>').value;
                   var outAddress = document.getElementById('<%= empoutaddress.ClientID %>').value;

                   //var empiconout = "/pics/emp_outicon.png";
                   //var empiconin = "/pics/emp_inicon.png";
                   var empiconin = "http://maps.gstatic.com/mapfiles/ms2/micons/grn-pushpin.png ";
                   var empiconout = " http://maps.gstatic.com/mapfiles/ms2/micons/red-pushpin.png";
                   if (latLngString === null || latLngString.trim() === "") { 
                       marker.setPosition({ lat: lat, lng: lng });
                       marker2.setMap(null);
                   }
                   else {
                       marker.setPosition({ lat: newLat, lng: newLng });
                       //marker.setAnimation(google.maps.Animation.BOUNCE);
                       marker.setIcon(empiconin);
                       marker.setDraggable(false);
                       marker.setTitle('In Location: '+inAddress+'\nIn Time: '+inTime);
                       marker2.setPosition({ lat: lat, lng: lng })
                   }
                    if (outlatLngString === null || outlatLngString.trim() === "") { 
                        marker2.setPosition({ lat: lat, lng: lng });
                         
                   }
                   else {
                        marker2.setPosition({ lat: outLat, lng: outLng });
                        //marker2.setAnimation(google.maps.Animation.BOUNCE);
                       marker2.setIcon(empiconout);
                       marker2.setDraggable(false);
                       marker2.setTitle('Out Location: ' +outAddress+'\nOut Time: '+outTime);
                        
                   }


                   google.maps.event.addListener(marker, 'dragend', function () {
                       markerPosition(marker.getPosition());
                       geocodeLatLng(marker.getPosition());
                   });
                   google.maps.event.addListener(marker2, 'dragend', function () {
                       markerPosition(marker2.getPosition());
                       geocodeLatLng(marker2.getPosition());
                   });

                   //markerPosition(marker.getPosition());
                   //geocodeLatLng(marker.getPosition()
               }

           });    
           }

        function markerPosition(latLng) {
            var positionString = latLng.lat() + ',' + latLng.lng();
            document.getElementById('<%= LatLongitude.ClientID %>').value = positionString;
        }


        function geocodeLatLng(latlng) {
            var geocoder = new google.maps.Geocoder();

            geocoder.geocode({ 'location': latlng }, function (results, status) {
                if (status == google.maps.GeocoderStatus.OK) {
                    if (results[0]) {
                        var address = results[0].formatted_address;
                        document.getElementById('TextBox2').value = address;

                    }
                }
            });
        }

    </script>   
</head>
<body>
    <form id="form1" runat="server">
        <div class="auto-style1">
        <div class="container-fluid">
         <h2 class="d-block p-3 bg-dark text-white text-start">
                          <asp:DropDownList ID="DropDownList2" runat="server" style= "text-align: left" class="btn btn-dark dropdown-toggle" type="button" data-bs-toggle="dropdown" aria-expanded="false" AutoPostBack="True" Font-Size="Large" OnSelectedIndexChanged="DropDownList2_SelectedIndexChanged">
                          
                               <asp:ListItem Value="1">Admin Page</asp:ListItem>
                               <asp:ListItem Value="2">Employee Page</asp:ListItem>
                                              
                 
                         </asp:DropDownList>
                          
                          &nbsp; &nbsp; GEOTAG BASED ATTENDANCE SYSTEM USING GOOGLE MAPS API
                     </h2>
      
        
            <div class="col" style="background-color: #D3D3D3; color: black; text-align: center; height:615px">
                  
                       <div class="alert alert-primary" style="text-align:center; margin-left:0px; margin-right:0px;"  role="alert"> Choose an employee and use the marker in the center to pin the employee's location for attendance!</div>
								 
                                    <div class="container  border border border-4"  style=" background-color: white; color: black; text-align: center; height:515px; width:1400px" >
                                    <div class="row">
                                           <div class="col-md-8">
                                           <div class="text-center my-4">
                                           <asp:Label ID="Label2" runat="server" Text="Map Preview" Font-Bold="True"></asp:Label>
                                            <div id="preview" style="height: 440px; width: 725px;" class="border border border-4">
                                            </div>
                                            </div>
                                            </div>
                                            <div class="col-md-4">
                                            <div class="text-start">
                                                <br /><br />
                                                <asp:Label ID="Label1" runat="server" Text="Employee ID : " class="form-label" Font-Bold="True"></asp:Label> &nbsp;
                                                <asp:DropDownList ID="DropDownList1" runat="server" style= "text-align: left" class="btn btn-dark dropdown-toggle" type="button" data-bs-toggle="dropdown" aria-expanded="false" AutoPostBack="True" DataSourceID="SqlDataSource1" DataTextField="EmpInfo" DataValueField="Emp_ID" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged  " AppendDataBoundItems="True">
                                                <asp:ListItem Text="Select" Value="0" />
                                                </asp:DropDownList>
                                                <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:NEWSHAMEEConnectionString2 %>" SelectCommand="SELECT [Emp_ID], CAST([Emp_ID] AS NVARCHAR(50)) + ' - ' + [Name] AS EmpInfo FROM [Emp_Details]"></asp:SqlDataSource>
                                                <br />
                                                <br />
                                                <asp:Label ID="Label3" runat="server" Text="Branch" Font-Bold="True"> </asp:Label>
                                                <asp:TextBox ID="TextBox1" runat="server" class="form-control"  placeholder="Employee Branch" ReadOnly="True"></asp:TextBox>
                                                <br /> 
                                                <asp:Label ID="Label5" runat="server" Text="Employee Current Location"  class="form-label" Font-Bold="True"></asp:Label>
                                                <asp:TextBox ID="TextBox2" runat="server" class="form-control"  TextMode="MultiLine" Rows="5" AutoPostBack="True">

                                                </asp:TextBox>&nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp;&nbsp; &nbsp;&nbsp; &nbsp;
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Employee Location Required" ControlToValidate="TextBox2" ForeColor="Red"></asp:RequiredFieldValidator>
                                                <br />&nbsp; &nbsp; 
                                                <asp:Label ID="Label6" runat="server" style ="text-align:center" class="form-label" Font-Bold="True" ForeColor="Red"></asp:Label><br />
                                               &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp;&nbsp; &nbsp;
                                                <asp:Label ID="Label4" runat="server" style ="text-align:center" class="form-label" Font-Bold="True" ForeColor="Red"></asp:Label>
                                                
                                                <asp:HiddenField ID="LatLongitude" runat="server" ClientIDMode="Static"  />
                                                 <asp:HiddenField ID="newlatilngi" runat="server" ClientIDMode="Static"  />
                                                <asp:HiddenField ID="outlatilngi" runat="server" ClientIDMode="Static"  />
                                                <asp:HiddenField ID="empintime" runat="server" ClientIDMode="Static"  />
                                                <asp:HiddenField ID="empouttime" runat="server" ClientIDMode="Static"  />
                                                 <asp:HiddenField ID="empinaddress" runat="server" ClientIDMode="Static"  />
                                                 <asp:HiddenField ID="empoutaddress" runat="server" ClientIDMode="Static"  />

                                                
                                                </div>
                                                
                                                <br />
                                                <asp:Button ID="PunchIn" runat="server" Text="Punch-In" class="btn btn-dark" OnClick="PunchIn_Click"/>
                                                &nbsp; <asp:Button ID="PunchOut" runat="server" Text="Punch-Out" class="btn btn-dark" Enabled="False" OnClick="PunchOut_Click"   />
                                                <br /><br />
                                                

                                         </div>
                                   </div>
                             </div> 
                      </div>
             </div>
     </div>
    <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.9.2/dist/umd/popper.min.js" integrity="sha384-IQsoLXl5PILFhosVNubq5LC7Qb9DXgDA9i+tQ8Zj3iwWAwPtgFTxbJ8NT4GN1R8p" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.min.js" integrity="sha384-cVKIPhGWiC2Al4u+LWgxfKTRIcfu0JTxR+EQDz/bgldoEyl4H0zUF0QKbrJ0EcQF" crossorigin="anonymous"></script>
   
    </form>
    </body>
</html>