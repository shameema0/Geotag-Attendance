<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="MajorProject.Login" %>

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
            height: 435px;
        }
        .auto-style2 {
            width: 86px;
            height: 19px;
            position: absolute;
            left: 357px;
            top: 222px;
        }
    </style>

   
</head>
<body style="height: 541px">
    <form id="form1" runat="server">
        <div class="auto-style1">
         <div class="container-fluid">
        <h2 class="d-block p-4 bg-dark text-white text-center">GEOTAG BASED ATTENDANCE SYSTEM USING GOOGLE MAPS API</h2>
        
            
            <div class="col" style=" background-color: #D3D3D3; color: black; text-align: center; height:500px">
                <div class="alert alert-primary" style="text-align:center; margin-left:0px; margin-right:0px;"  role="alert">Login - Enter valid  credentials to login as Employee!</div>

                
                    <div class="container">
                    
                       
                        <div class="row">
                            <div class="col">
                                <div class="text-center mx-5">
								<br />
                               <div class="container border border border-4"  style=" background-color: white; color: black; text-align: center; height:335px; width:350px" >
                                    <div class="text-start my-5">
                           <asp:Label ID="Label1" runat="server" Text="Username : " class="form-label" ></asp:Label>
                                        <br />
                                        <asp:TextBox ID="TextBox1" runat="server"  class="form-control"  placeholder="Enter Username" ></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ControlToValidate="TextBox1" ForeColor="#F90000"></asp:RequiredFieldValidator>
                                        <br />
                                          <asp:Label ID="Label2" runat="server" Text="Password : " class="form-label" ></asp:Label>
                                        <br />
                                        <asp:TextBox ID="TextBox2" runat="server"  class="form-control"  placeholder="Enter Password" TextMode="Password"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="*" ControlToValidate="TextBox2" ForeColor="Red"></asp:RequiredFieldValidator>
                                        <br />
                                        <div class="text-center my-2">
                                        <asp:Button ID="Button1" runat="server" Text="Login" class="btn btn-dark" OnClick="Button1_Click" />
                                            <br /><br />
                                            <asp:Label ID="Label3" runat="server" ForeColor="Red"></asp:Label>
                                            </div>
                                      </div>
                                 </div>
                              </div>
	                        </div>
                          </div>                     
                        </div>
                    </div>
        
    </div>

    <script src="https://cdn.jsdelivr.net/npm/@popperjs/core@2.9.2/dist/umd/popper.min.js" integrity="sha384-IQsoLXl5PILFhosVNubq5LC7Qb9DXgDA9i+tQ8Zj3iwWAwPtgFTxbJ8NT4GN1R8p" crossorigin="anonymous"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.0.2/dist/js/bootstrap.min.js" integrity="sha384-cVKIPhGWiC2Al4u+LWgxfKTRIcfu0JTxR+EQDz/bgldoEyl4H0zUF0QKbrJ0EcQF" crossorigin="anonymous"></script>
        </div>
        
    </form>
     </body>
</html>