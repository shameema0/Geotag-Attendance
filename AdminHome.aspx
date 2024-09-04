<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminHome.aspx.cs" Inherits="MajorProject.AdminHome" %>

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

    <style type="text/css">
        .auto-style1 {
            height: 331px;
        }
        
    </style>
    <script src="https://maps.googleapis.com/maps/api/js?key=AIzaSyCCgwzm9uzuvijnFWprbSJb-rJN-9GbhHU&libraries=places&callback=initMap" async defer></script>
   <script>
       var map;
       var marker;

       function initMap() {
           var initialLocation = { lat: 13.007780496481956, lng: 80.23899847554192 };

           map1 = new google.maps.Map(document.getElementById('preview'), {
               center: initialLocation,
               zoom: 13
           });

           var dropdown = document.getElementById('DropDownList1');
           var selectedLocation = dropdown.options[dropdown.selectedIndex].value;

           var latLngString = document.getElementById('<%= newlatilngi.ClientID %>').value;
           var latLngArray = latLngString.split(',');
           var newLat = parseFloat(latLngArray[0]);
           var newLng = parseFloat(latLngArray[1]);
           var geocoder = new google.maps.Geocoder();

           geocoder.geocode({ 'address': selectedLocation }, function (results, status) {
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


                   if (latLngString === null || latLngString.trim() === "") {
                       marker.setPosition({ lat: lat, lng: lng });
                   }
                   else {
                       marker.setPosition({ lat: newLat, lng: newLng });
                   }

                   google.maps.event.addListener(marker, 'dragend', function () {
                       markerPosition(marker.getPosition());
                       geocodeLatLng(marker.getPosition());
                   });

                   //markerPosition(marker.getPosition());
                   //geocodeLatLng(marker.getPosition());
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
                    document.getElementById('TextBox1').value = address;
                    var postalCode = getPincode(results[0].address_components);
                    document.getElementById('<%= PostalCode.ClientID %>').value = postalCode;
                    var areaName = getAreaName(results[0].address_components);
                    document.getElementById('<%= areaname.ClientID %>').value = areaName;

                }
            }
        });
       }
       function getPincode(addressComponents) {
           for (var i = 0; i < addressComponents.length; i++) {
               var types = addressComponents[i].types;
               if (types.includes('postal_code')) {
                   return addressComponents[i].short_name;
               }
           }
           return 0;
       }

       function getAreaName(addressComponents) {
           for (var i = 0; i < addressComponents.length; i++) {
               var types = addressComponents[i].types;
               if (types.includes('sublocality_level_1')) {
                   return addressComponents[i].long_name;
               }
           }
           return 0;
       }
   </script>
                
</head>
<body style="height: 478px">
    <form id="form1" runat="server">
        <div class="auto-style1">
        <div class="container-fluid">
                
                      <h2 class="d-block p-3 bg-dark text-white text-start">
                          <asp:DropDownList ID="DropDownList2" runat="server"  style= "text-align: left" class="btn btn-dark dropdown-toggle" type="button" data-bs-toggle="dropdown" aria-expanded="false" AutoPostBack="True" Font-Size="Large" OnSelectedIndexChanged="DropDownList2_SelectedIndexChanged1" >
                                        
                               <asp:ListItem Value="1">Admin Page</asp:ListItem>
                               <asp:ListItem Value="2">Employee Page</asp:ListItem>
                                              
                           </asp:DropDownList>
                          
                          &nbsp; &nbsp; GEOTAG BASED ATTENDANCE SYSTEM USING GOOGLE MAPS API
                     </h2>
      
         
              
            <div class="col" style=" background-color: #D3D3D3; color: black; text-align: center; height:575px">
                <div class="alert alert-primary" style="text-align:center; margin-left:0px; margin-right:0px;"  role="alert">Select a location and use the marker to pin the company's branch location!</div>

                                <div class="container  border border border-4"  style=" background-color: white; color: black; text-align: center; height:485px; width:1400px" >
                                   <div class="row">
                                      <div class="col-md-8">
                                        <div class="text-start my-4">
                                          <asp:Label ID="Label1" runat="server" Text="Location/Branch : "  Font-Bold="True"></asp:Label> &nbsp;
                                           <asp:DropDownList ID="DropDownList1" runat="server" style= "text-align: left" class="btn btn-dark dropdown-toggle" type="button" data-bs-toggle="dropdown" aria-expanded="false" DataSourceID="SqlDataSource1" DataTextField="BranchName" DataValueField="BranchName" AutoPostBack="True" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged" AppendDataBoundItems="True">
                                             <asp:ListItem Text="Select" Value="0" />
                                              </asp:DropDownList>
                                              <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:NEWSHAMEEConnectionString %>" SelectCommand="SELECT [BranchName] FROM [BranchLoc]"></asp:SqlDataSource>
                                              <br />
                                              <br />
                                              <div id="preview" style="height: 355px; width: 725px;" class="border border border-4">    
                                              </div>
                                              <div class="text-center mx-5">
                                              <asp:Label ID="Label2" runat="server" Text="Map Preview"  Font-Bold="True"></asp:Label>
                                              </div>
                                              </div>
                                              </div>
                                              <div class="col-md-4">
                                              <div class="text-center">
                                              <br /><br />
                                              <br />
                                              <asp:Label ID="Label3" runat="server" Text="MapAddress" Font-Bold="True"></asp:Label>
                                              <br /><br />
                                              <asp:TextBox ID="TextBox1" runat="server"  class="form-control"   TextMode="MultiLine" Rows="5" AutoPostBack="True" >

                                              </asp:TextBox>
                                              <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Branch Location is required" ControlToValidate="TextBox1" ForeColor="Red"></asp:RequiredFieldValidator>
                                              <br />
                                              <br />
                                               <asp:HiddenField ID="LatLongitude" runat="server" ClientIDMode="Static"  />
                                               <asp:HiddenField ID="PostalCode" runat="server" ClientIDMode="Static"  />
                                               <asp:HiddenField ID="areaname" runat="server" ClientIDMode="Static"  />
                                               <asp:HiddenField ID="latilngi" runat="server" ClientIDMode="Static"  />
                                               <asp:HiddenField ID="newlatilngi" runat="server" ClientIDMode="Static"  />

                                                 
                                               <%--<asp:Button ID="Button1" runat="server" Text="Confirm" class="btn btn-dark" OnClick="Button1_Click"/>--%>
                                               
                                                  
                                               <button type="button" class="btn btn-dark" data-bs-toggle="modal" data-bs-target="#staticBackdrop">
                                                  Confirm</button>

                                                <!-- Modal -->
                                                <div class="modal fade" id="staticBackdrop" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="staticBackdropLabel" aria-hidden="true">
                                                  <div class="modal-dialog">
                                                    <div class="modal-content">
                                                      <div class="modal-header">
                                                        <h5 class="modal-title" id="staticBackdropLabel">Location Confirmation</h5>
                                                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                                                      </div>
                                                      <div class="modal-body">
                                                        Are you sure with pinned location?
                                                      </div>
                                                      <div class="modal-footer">
                                                          <asp:Button ID="Button1" runat="server" Text="Yes" class="btn btn-primary" OnClick="Button1_Click"/>
        
                                                          <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">No</button>
                                                      </div>
                                                    </div>
                                                  </div>
                                                </div>
                                                  <br /><br />
                                               <asp:Label ID="Label4" runat="server"></asp:Label>
                                               </div>
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
