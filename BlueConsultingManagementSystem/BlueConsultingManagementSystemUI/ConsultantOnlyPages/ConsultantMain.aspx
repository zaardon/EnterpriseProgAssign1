﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConsultantMain.aspx.cs" Inherits="BlueConsultingManagementSystemUI.ConsultantOnlyPages.ConsultantMain" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    Welcome Mr Consultant
    </div>
        <table style="width:300px">
<tr>
  <td>date</td>
  <td>
      <asp:Calendar ID="Calendar1" runat="server"></asp:Calendar>
      </td> 
</tr>
<tr>
  <td>location</td>
  <td>
      <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox></td> 
</tr>
            <tr>
  <td>description</td>
  <td>
      <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox></td> 
</tr>
            <tr>
  <td>amount</td>
  <td>
      <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>  </td> 
</tr>
            <tr>
  <td>currency</td>
  <td>
      <asp:DropDownList ID="DropDownList1" runat="server">
          <asp:ListItem>AUD</asp:ListItem>
          <asp:ListItem>CNY</asp:ListItem>
          <asp:ListItem>ERU</asp:ListItem>
          <asp:ListItem>USD</asp:ListItem>
      </asp:DropDownList></td> 
</tr>
            <tr>
  <td>PDF RECEIPT</td>
  <td>
      <asp:FileUpload ID="FileUpload1" runat="server" /></td> 
</tr>


</table>
       

        

    </form>
</body>
</html>
